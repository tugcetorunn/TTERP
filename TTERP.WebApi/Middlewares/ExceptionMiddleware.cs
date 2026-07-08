using FluentValidation;
using TTERP.Shared.Extensions;

namespace TTERP.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var response = ex.ToValidationResponse<object>();
                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }

            catch (Exception ex)
            {
                var response = ex.ToResponse<object>();
                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }

        }
    }
}
