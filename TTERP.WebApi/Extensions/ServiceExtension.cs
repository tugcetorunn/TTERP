using System.Globalization;
using TTERP.Application.Interfaces;
using TTERP.Domain.Interfaces.ServiceInterfaces;
using TTERP.Persistence.Services;
using TTERP.WebApi.SignalR;

namespace TTERP.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationDispatcher, SignalRNotificationDispatcher>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton(new CultureInfo("tr-TR"));
        }
    }
}
