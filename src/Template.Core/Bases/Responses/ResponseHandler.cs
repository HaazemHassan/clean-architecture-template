namespace Template.Core.Bases.Responses {
    public class ResponseHandler {
        public Response Success(string? message = null, object? meta = null) {
            return new Response() {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Operation Succeeded",
                Meta = meta
            };
        }

        public Response<T> Success<T>(T entity, string? message = null, object? meta = null) {
            return new Response<T>() {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Operation Succeeded",
                Meta = meta
            };
        }

        public Response Created(string? message = null, object? meta = null) {
            return new Response() {
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = message ?? "Created Successfully",
                Meta = meta
            };
        }

        public Response<T> Created<T>(T entity, string? message = null, object? meta = null) {
            return new Response<T>() {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = message ?? "Created Successfully",
                Meta = meta
            };
        }



        public Response Updated(string? message = null, object? meta = null) {
            return new Response() {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Updated Successfully",
                Meta = meta
            };
        }

        public Response<T> Updated<T>(T entity, string? message = null, object? meta = null) {
            return new Response<T>() {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Updated Successfully",
                Meta = meta
            };
        }


        public Response Deleted(string? message = null, object? meta = null) {

            return new Response() {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Deleted Successfully",
                Meta = meta

            };
        }
        public Response<T> Deleted<T>(T entity, string? message = null, object? meta = null) {

            return new Response<T>() {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message is null ? "Deleted Successfully" : message,
                Meta = meta
            };
        }




        public Response<T> Unauthorized<T>(string? message = null, object? meta = null, string? errorCode = null) {
            return new Response<T>() {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message ?? "Unauthorized Access",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Response<T> Forbid<T>(string? message = null, object? meta = null, string? errorCode = null) {
            return new Response<T>() {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Succeeded = false,
                Message = message ?? "Access Forbidden",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Response<T> BadRequest<T>(string? Message = null, object? meta = null, string? errorCode = null) {
            return new Response<T>() {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message,
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Response<T> Conflict<T>(string? Message = null, object? meta = null, string? errorCode = null) {
            return new Response<T>() {
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Succeeded = false,
                Message = Message ?? "Conflict",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Response<T> NotFound<T>(string? message = null, object? meta = null, string? errorCode = null) {
            return new Response<T>() {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? "Not Found",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Response<T> FromServiceResult<T>(ServiceOperationResult<T>? serviceResult) {
            return serviceResult!.Status switch {
                Enums.ServiceOperationStatus.Succeeded => Success(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Created => Created(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Updated => Updated(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Deleted => Deleted(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.NotFound => NotFound<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.AlreadyExists => Conflict<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Unauthorized => Unauthorized<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Forbidden => Forbid<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.InvalidParameters => BadRequest<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }

        public Response FromServiceResult(ServiceOperationResult? serviceResult) {
            return serviceResult!.Status switch {
                Enums.ServiceOperationStatus.Succeeded => Success(serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Created => Created(serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Updated => Updated(serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.Deleted => Deleted(serviceResult.Message, serviceResult.Meta),
                Enums.ServiceOperationStatus.NotFound => NotFound<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.AlreadyExists => Conflict<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Unauthorized => Unauthorized<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Forbidden => Forbid<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.InvalidParameters => BadRequest<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }

        public Response<TResponse> FromServiceResult<TResponse>(ServiceOperationResult serviceResult) {
            return serviceResult.Status switch {
                Enums.ServiceOperationStatus.NotFound => NotFound<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.AlreadyExists => Conflict<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Unauthorized => Unauthorized<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Forbidden => Forbid<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.InvalidParameters => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                Enums.ServiceOperationStatus.Failed => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }




    }
}
