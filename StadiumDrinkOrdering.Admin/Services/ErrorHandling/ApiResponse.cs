using System.Net;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    /// <summary>
    /// Wrapper class for API responses that includes success status and error information
    /// </summary>
    /// <typeparam name="T">The type of data returned by the API</typeparam>
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public bool RequiresAuth { get; set; }
        public bool IsRetryable { get; set; }
        public string? Endpoint { get; set; }

        /// <summary>
        /// Creates a successful API response
        /// </summary>
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failed API response with error details
        /// </summary>
        public static ApiResponse<T> Failure(HttpStatusCode statusCode, string? errorMessage = null, string? endpoint = null)
        {
            var errorInfo = ErrorMessageMappings.GetErrorInfo(statusCode, endpoint);

            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage ?? errorInfo.Message,
                RequiresAuth = errorInfo.RequiresAuth,
                IsRetryable = errorInfo.IsRetryable,
                Endpoint = endpoint
            };
        }

        /// <summary>
        /// Creates a failed API response from an exception
        /// </summary>
        public static ApiResponse<T> Exception(Exception exception, string? endpoint = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorMessage = exception.Message,
                Endpoint = endpoint,
                IsRetryable = IsRetryableException(exception)
            };
        }

        private static bool IsRetryableException(Exception exception)
        {
            return exception is TimeoutException ||
                   exception is TaskCanceledException ||
                   exception is HttpRequestException ||
                   (exception is OperationCanceledException && !(exception is TaskCanceledException));
        }
    }

    /// <summary>
    /// Non-generic version for operations that don't return data
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Creates a successful API response without data
        /// </summary>
        public static ApiResponse Success()
        {
            return new ApiResponse
            {
                IsSuccess = true
            };
        }

        /// <summary>
        /// Creates a failed API response without data
        /// </summary>
        public new static ApiResponse Failure(HttpStatusCode statusCode, string? errorMessage = null, string? endpoint = null)
        {
            var errorInfo = ErrorMessageMappings.GetErrorInfo(statusCode, endpoint);

            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage ?? errorInfo.Message,
                RequiresAuth = errorInfo.RequiresAuth,
                IsRetryable = errorInfo.IsRetryable,
                Endpoint = endpoint
            };
        }

        /// <summary>
        /// Creates a failed API response from an exception without data
        /// </summary>
        public new static ApiResponse Exception(Exception exception, string? endpoint = null)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = exception.Message,
                Endpoint = endpoint,
                IsRetryable = IsRetryableException(exception)
            };
        }

        private static bool IsRetryableException(Exception exception)
        {
            return exception is TimeoutException ||
                   exception is TaskCanceledException ||
                   exception is HttpRequestException ||
                   (exception is OperationCanceledException && !(exception is TaskCanceledException));
        }
    }
}