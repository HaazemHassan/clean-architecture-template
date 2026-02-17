using System.Net;

namespace Template.Application.Common.Responses
{


    public class Result
    {

        public Result()
        {
            Errors = new();
        }
        public Result(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new();
        }
        public Result(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
            Errors = new();
        }
        public HttpStatusCode StatusCode { get; set; }
        public object? Meta { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; }
        public string? ErrorCode { get; set; }

    }
    public class Result<T> : Result
    {
        public Result() : base()
        {
        }
        public Result(T data, string message)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public T? Data { get; set; }
    }
}
