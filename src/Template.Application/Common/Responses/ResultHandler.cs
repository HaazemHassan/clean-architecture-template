using Template.Application.Enums;

namespace Template.Application.Common.Responses
{
    public class ResultHandler
    {
        public Result Success(string? message = null, object? meta = null)
        {
            return new Result()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Operation Succeeded",
                Meta = meta
            };
        }

        public Result<T> Success<T>(T entity, string? message = null, object? meta = null)
        {
            return new Result<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Operation Succeeded",
                Meta = meta
            };
        }

        public Result Created(string? message = null, object? meta = null)
        {
            return new Result()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = message ?? "Created Successfully",
                Meta = meta
            };
        }

        public Result<T> Created<T>(T entity, string? message = null, object? meta = null)
        {
            return new Result<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = message ?? "Created Successfully",
                Meta = meta
            };
        }



        public Result Updated(string? message = null, object? meta = null)
        {
            return new Result()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Updated Successfully",
                Meta = meta
            };
        }

        public Result<T> Updated<T>(T entity, string? message = null, object? meta = null)
        {
            return new Result<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Updated Successfully",
                Meta = meta
            };
        }


        public Result Deleted(string? message = null, object? meta = null)
        {

            return new Result()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Deleted Successfully",
                Meta = meta

            };
        }
        public Result<T> Deleted<T>(T entity, string? message = null, object? meta = null)
        {

            return new Result<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message is null ? "Deleted Successfully" : message,
                Meta = meta
            };
        }




        public Result<T> Unauthorized<T>(string? message = null, object? meta = null, string? errorCode = null)
        {
            return new Result<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message ?? "Unauthorized Access",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Result<T> Forbid<T>(string? message = null, object? meta = null, string? errorCode = null)
        {
            return new Result<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Succeeded = false,
                Message = message ?? "Access Forbidden",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Result<T> BadRequest<T>(string? Message = null, object? meta = null, string? errorCode = null)
        {
            return new Result<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message,
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Result<T> Conflict<T>(string? Message = null, object? meta = null, string? errorCode = null)
        {
            return new Result<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Succeeded = false,
                Message = Message ?? "Conflict",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Result<T> NotFound<T>(string? message = null, object? meta = null, string? errorCode = null)
        {
            return new Result<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? "Not Found",
                Meta = meta,
                ErrorCode = errorCode
            };
        }

        public Result<T> FromServiceResult<T>(ServiceOperationResult<T>? serviceResult)
        {
            return serviceResult!.Status switch
            {
                ServiceOperationStatus.Succeeded => Success(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Created => Created(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Updated => Updated(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Deleted => Deleted(serviceResult.Data!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.NotFound => NotFound<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.AlreadyExists => Conflict<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Unauthorized => Unauthorized<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Forbidden => Forbid<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.InvalidParameters => BadRequest<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<T>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }

        public Result FromServiceResult(ServiceOperationResult? serviceResult)
        {
            return serviceResult!.Status switch
            {
                ServiceOperationStatus.Succeeded => Success(serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Created => Created(serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Updated => Updated(serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Deleted => Deleted(serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.NotFound => NotFound<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.AlreadyExists => Conflict<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Unauthorized => Unauthorized<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Forbidden => Forbid<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.InvalidParameters => BadRequest<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<string>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }

        public Result<TResponse> FromServiceResult<TResponse>(ServiceOperationResult serviceResult)
        {
            return serviceResult.Status switch
            {
                ServiceOperationStatus.Succeeded => Success(default(TResponse)!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Created => Created(default(TResponse)!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Updated => Updated(default(TResponse)!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.Deleted => Deleted(default(TResponse)!, serviceResult.Message, serviceResult.Meta),
                ServiceOperationStatus.NotFound => NotFound<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.AlreadyExists => Conflict<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Unauthorized => Unauthorized<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Forbidden => Forbid<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.InvalidParameters => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                ServiceOperationStatus.Failed => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode),
                _ => BadRequest<TResponse>(serviceResult.Message, serviceResult.Meta, serviceResult.ErrorCode)
            };
        }






    }
}
