using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Shared.Extensions
{
    public static class ExceptionExtension
    {
        public static Response<T> ToResponse<T>(this Exception ex, int statusCode = 500) where T : class
        {
            var detailMessage = ex.GetBaseException().Message;
            var message = ex switch
            {
                UnauthorizedAccessException => "Erişim yetkiniz yok",
                ArgumentNullException => "Zorunlu alan eksik",
                KeyNotFoundException => "Kayıt bulunamadı",
                DbUpdateException => detailMessage,
                _ => ex.Message ?? "Beklenmeyen bir hata oluştu"
            };

            return new Response<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { detailMessage },
                Data = default
            };
        }

        public static Response<T> ToValidationResponse<T>(this FluentValidation.ValidationException ex) where T : class
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

            return new Response<T>
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "Validation failed",
                Errors = errors,
                Data = default
            };
        }

        public static Response<List<string>> ToValidationErrorList(this FluentValidation.ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

            return new Response<List<string>>
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "Validation failed",
                Errors = errors,
                Data = errors
            };
        }
    }
}
