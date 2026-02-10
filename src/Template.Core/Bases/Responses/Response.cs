using System.Net;

namespace Template.Core.Bases.Responses {


    public class Response {

        public Response() {
        }
        public Response(string message) {
            Succeeded = false;
            Message = message;
        }
        public Response(string message, bool succeeded) {
            Succeeded = succeeded;
            Message = message;
        }
        public HttpStatusCode StatusCode { get; set; }
        public object? Meta { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public string? ErrorCode { get; set; }

    }
    public class Response<T> : Response {
        public Response() : base() {
        }
        public Response(T data, string message) {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public T? Data { get; set; }
    }
}
