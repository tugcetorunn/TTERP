using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Shared.Models
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        // veri dönen success
        public static Response<T> Success(T data, int statusCode = 200, string? message = null)
        {
            return new Response<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        // veri dönmeyen success (update, delete işlemleri için)
        public static Response<T> Success(int statusCode = 200, string? message = null)
        {
            return new Response<T>
            {
                Data = default,
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Response<T> Fail(int statusCode = 400, string message = "", params string[] errors)
        {
            return new Response<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Errors = errors.ToList(),
                Message = message
            };
        }
    }
}
