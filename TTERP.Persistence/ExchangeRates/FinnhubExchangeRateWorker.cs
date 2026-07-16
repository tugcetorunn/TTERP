using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TTERP.Application.Interfaces;
using TTERP.Persistence.ExchangeRates.Models;

namespace TTERP.Persistence.ExchangeRates;

public sealed class FinnhubExchangeRateWorker : BackgroundService
{
    private readonly FinnhubOptions _options;
    private readonly IExchangeRateStore _exchangeRateStore;
    private readonly IExchangeRatePublisher _publisher;
    private readonly ILogger<FinnhubExchangeRateWorker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly TimeSpan ReconnectDelay =
        TimeSpan.FromSeconds(10);

    public FinnhubExchangeRateWorker(
        IOptions<FinnhubOptions> options,
        IExchangeRateStore exchangeRateStore,
        IExchangeRatePublisher publisher,
        ILogger<FinnhubExchangeRateWorker> logger,
        IHttpClientFactory httpClientFactory)
    {
        _options = options.Value;
        _exchangeRateStore = exchangeRateStore;
        _publisher = publisher;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            _logger.LogWarning(
                "Finnhub API anahtarı bulunamadığı için kur servisi başlatılmadı.");

            return;
        }

        if (string.IsNullOrWhiteSpace(_options.WebSocketUrl))
        {
            _logger.LogError(
                "Finnhub WebSocketUrl ayarı bulunamadı.");

            return;
        }

        if (!Uri.TryCreate(
            _options.WebSocketUrl,
            UriKind.Absolute,
            out var webSocketUri))
        {
            _logger.LogError(
                "Finnhub WebSocketUrl geçersiz: {WebSocketUrl}",
                _options.WebSocketUrl);

            return;
        }

        //await LoadInitialRatesAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndReceiveAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Finnhub WebSocket bağlantısında hata oluştu.");
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(ReconnectDelay, stoppingToken);
            }
        }
    }

    private async Task ConnectAndReceiveAsync(
    CancellationToken cancellationToken)
    {
        using var socket = new ClientWebSocket();

        var baseUrl = _options.WebSocketUrl.TrimEnd('/');

        var socketUri = new Uri(
            $"{baseUrl}?token={Uri.EscapeDataString(_options.ApiKey)}",
            UriKind.Absolute);

        _logger.LogInformation(
            "Finnhub WebSocket bağlantısı kuruluyor: {Host}",
            socketUri.Host);

        await socket.ConnectAsync(socketUri, cancellationToken);

        _logger.LogInformation(
            "Finnhub WebSocket bağlantısı kuruldu.");

        foreach (var symbol in _options.Symbols)
        {
            await SubscribeAsync(
                socket,
                symbol,
                cancellationToken);
        }

        await ReceiveMessagesAsync(socket, cancellationToken);
    }

    private async Task SubscribeAsync(
        ClientWebSocket socket,
        string symbol,
        CancellationToken cancellationToken)
    {
        var request = JsonSerializer.Serialize(new
        {
            type = "subscribe",
            symbol
        });

        var bytes = Encoding.UTF8.GetBytes(request);

        await socket.SendAsync(
            bytes,
            WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken);

        _logger.LogInformation(
        "Finnhub sembol aboneliği gönderildi: {Symbol}",
        symbol);
    }

    private async Task ReceiveMessagesAsync(
        ClientWebSocket socket,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[16 * 1024];

        while (
            socket.State == WebSocketState.Open &&
            !cancellationToken.IsCancellationRequested)
        {
            using var messageStream = new MemoryStream();

            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(
                    buffer,
                    cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocketAsync(
                        socket,
                        cancellationToken);

                    return;
                }

                messageStream.Write(
                    buffer,
                    0,
                    result.Count);
            }
            while (!result.EndOfMessage);

            if (result.MessageType != WebSocketMessageType.Text)
            {
                continue;
            }

            var json = Encoding.UTF8.GetString(
                messageStream.ToArray());

            await ProcessMessageAsync(
                json,
                cancellationToken);
        }
    }

    private async Task ProcessMessageAsync(
        string json,
        CancellationToken cancellationToken)
    {
        FinnhubWebSocketMessage? message;

        try
        {
            message = JsonSerializer.Deserialize<FinnhubWebSocketMessage>(
                json);
        }
        catch (JsonException exception)
        {
            _logger.LogWarning(
                exception,
                "Finnhub mesajı ayrıştırılamadı: {Message}",
                json);

            return;
        }

        if (
            message?.Type != "trade" ||
            message.Data is null)
        {
            return;
        }

        foreach (var trade in message.Data)
        {
            var updatedAt = DateTimeOffset.FromUnixTimeMilliseconds(
                trade.Timestamp);

            var rate = _exchangeRateStore.AddOrUpdate(
                trade.Symbol,
                trade.Price,
                updatedAt);

            await _publisher.PublishAsync(
                rate,
                cancellationToken);
        }
    }

    private static async Task CloseSocketAsync(
        ClientWebSocket socket,
        CancellationToken cancellationToken)
    {
        if (
            socket.State == WebSocketState.Open ||
            socket.State == WebSocketState.CloseReceived)
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Connection closed",
                cancellationToken);
        }
    }
}