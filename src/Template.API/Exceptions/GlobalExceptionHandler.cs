using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Responses;

namespace Template.API.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var requestPath = httpContext.Request.Path;
        var userId = httpContext.User?.Identity?.Name ?? "Anonymous";
        var isDevelopment = _environment.IsDevelopment();

        var responseModel = new Result<string>
        {
            Succeeded = false
        };

        int statusCode;

        switch (exception)
        {
            // ===== Application Authorization Exceptions =====

            case UnauthorizedException e:
            _logger.LogWarning(e, "Unauthorized (Application) - Path: {Path}, User: {User}", requestPath, userId);

            responseModel.Message = "Unauthorized";

            responseModel.StatusCode = HttpStatusCode.Unauthorized;
            statusCode = (int)HttpStatusCode.Unauthorized;
            break;

            case ForbiddenException e:
            _logger.LogWarning(e, "Forbidden (Application) - Path: {Path}, User: {User}", requestPath, userId);

            responseModel.Message = "You do not have permission to perform this action.";

            responseModel.StatusCode = HttpStatusCode.Forbidden;
            statusCode = (int)HttpStatusCode.Forbidden;
            break;

            // ===== Validation =====

            case ValidationException e:
            _logger.LogWarning(e,
                "Validation error - Path: {Path}, User: {User}, Errors: {Errors}",
                requestPath,
                userId,
                string.Join(", ", e.Errors.Select(err => err.ErrorMessage)));

            responseModel.Message = "Validation failed.";
            responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
            responseModel.Errors = [.. e.Errors.Select(err => err.ErrorMessage)];

            statusCode = (int)HttpStatusCode.UnprocessableEntity;
            break;

            // ===== Not Found =====

            case KeyNotFoundException e:
            _logger.LogWarning(e,
                "Resource not found - Path: {Path}, User: {User}",
                requestPath, userId);

            responseModel.Message = e.Message;
            responseModel.StatusCode = HttpStatusCode.NotFound;

            statusCode = (int)HttpStatusCode.NotFound;
            break;

            // ===== Database Update =====

            case DbUpdateException e:
            _logger.LogError(e,
                "Database update error - Path: {Path}, User: {User}, Inner: {Inner}",
                requestPath,
                userId,
                e.InnerException?.Message);

            responseModel.Message = isDevelopment
                ? $"Database update failed: {e.InnerException?.Message ?? e.Message}"
                : "An error occurred while updating the database.";

            responseModel.StatusCode = HttpStatusCode.BadRequest;
            statusCode = (int)HttpStatusCode.BadRequest;
            break;

            // ===== SQL Exception =====

            case SqlException e:
            _logger.LogError(e,
                "SQL error - Path: {Path}, User: {User}, Code: {Code}, Message: {Message}",
                requestPath,
                userId,
                e.Number,
                e.Message);

            responseModel.Message = isDevelopment
                ? $"Database error (Code: {e.Number}): {e.Message}"
                : "A database error occurred.";

            responseModel.StatusCode = HttpStatusCode.InternalServerError;
            statusCode = (int)HttpStatusCode.InternalServerError;
            break;

            // ===== Fallback =====

            default:
            _logger.LogError(exception,
                "Unhandled exception - Path: {Path}, User: {User}, Type: {Type}, Message: {Message}",
                requestPath,
                userId,
                exception.GetType().Name,
                exception.Message);

            responseModel.Message = isDevelopment
                ? exception.Message
                : "An unexpected error occurred.";

            responseModel.StatusCode = HttpStatusCode.InternalServerError;
            statusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

        return true;
    }
}
