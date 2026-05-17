using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; private set; }

        public static Result<T> Success(T data, int statusCode = 200)
            => new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static Result<T> Failure(string error, int statusCode = 400)
            => new() { IsSuccess = false, Error = error, StatusCode = statusCode };

        public static Result<T> NotFound(string error)
            => new() { IsSuccess = false, Error = error, StatusCode = 404 };
    }
}
