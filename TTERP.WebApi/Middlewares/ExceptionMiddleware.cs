using FluentValidation;
using TTERP.Shared.Extensions;

namespace TTERP.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
        } 

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "FluentValidation doğrulama hatası oluştu.");

                var response = ex.ToValidationResponse<object>();
                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir sunucu hatası oluştu.");

                Console.WriteLine(ex.ToString());

                var response = ex.ToResponse<object>();
                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }

        }
    }
}
