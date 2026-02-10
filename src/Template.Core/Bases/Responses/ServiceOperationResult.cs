using Template.Core.Enums;

namespace Template.Core.Bases.Responses;

public class ServiceOperationResult {
    public ServiceOperationStatus Status { get; }
    public string Message { get; }
    public string? ErrorCode { get; }
    public object? Meta { get; }

    public bool Succeeded => IsSuccessStatus(Status);

    protected ServiceOperationResult(
        ServiceOperationStatus status,
        string? message = null,
        string? errorCode = null,
        object? meta = null) {
        if (IsSuccessStatus(status) && !string.IsNullOrEmpty(errorCode))
            throw new ArgumentException(
                "Success result cannot have an ErrorCode.",
                nameof(errorCode));

        Status = status;
        ErrorCode = errorCode;
        Meta = meta;
        Message = message ?? GetDefaultMessage(status);
    }

    public static ServiceOperationResult Success(
        ServiceOperationStatus status = ServiceOperationStatus.Succeeded,
        string? message = null,
        object? meta = null) {
        if (!IsSuccessStatus(status))
            throw new ArgumentException(
                "Success method can only be called with success status.",
                nameof(status));

        return new(status, message, null, meta);
    }

    public static ServiceOperationResult Failure(
        ServiceOperationStatus status,
        string? message = null,
        string? errorCode = null,
        object? meta = null) {
        if (IsSuccessStatus(status))
            throw new ArgumentException(
                "Failure method cannot be called with success status.",
                nameof(status));

        return new(status, message, errorCode, meta);
    }

    protected static bool IsSuccessStatus(ServiceOperationStatus status) =>
        status == ServiceOperationStatus.Succeeded ||
        status == ServiceOperationStatus.Created ||
        status == ServiceOperationStatus.Updated ||
        status == ServiceOperationStatus.Deleted;

    private static string GetDefaultMessage(ServiceOperationStatus status) => status switch {
        ServiceOperationStatus.Succeeded => "Operation completed successfully.",
        ServiceOperationStatus.Created => "Resource created successfully.",
        ServiceOperationStatus.Updated => "Resource updated successfully.",
        ServiceOperationStatus.Deleted => "Resource deleted successfully.",
        ServiceOperationStatus.NotFound => "The requested resource was not found.",
        ServiceOperationStatus.AlreadyExists => "This record already exists.",
        ServiceOperationStatus.Unauthorized => "Unauthorized access.",
        ServiceOperationStatus.Forbidden => "You do not have permission to perform this action.",
        ServiceOperationStatus.InvalidParameters => "The provided parameters are invalid.",
        _ => "An unexpected error occurred."
    };

}

public sealed class ServiceOperationResult<T> : ServiceOperationResult {
    public T Data { get; }

    private ServiceOperationResult(
        ServiceOperationStatus status,
        T data,
        string? message = null,
        string? errorCode = null,
        object? meta = null)
        : base(status, message, errorCode, meta) {
        Data = data;
    }

    public static ServiceOperationResult<T> Success(
        T data,
        ServiceOperationStatus status = ServiceOperationStatus.Succeeded,
        string? message = null,
        object? meta = null) {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        if (!IsSuccessStatus(status))
            throw new ArgumentException(
                "Success method can only be called with success status.",
                nameof(status));

        return new(status, data, message, null, meta);
    }

    public new static ServiceOperationResult<T> Failure(
        ServiceOperationStatus status,
        string? message = null,
        string? errorCode = null,
        object? meta = null) {
        if (IsSuccessStatus(status))
            throw new ArgumentException(
                "Failure method cannot be called with success status.",
                nameof(status));

        return new(status, default!, message, errorCode, meta);
    }
}

