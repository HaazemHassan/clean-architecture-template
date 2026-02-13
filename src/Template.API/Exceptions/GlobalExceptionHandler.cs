using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Template.Core.Bases.Responses;

namespace Template.API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment) : IExceptionHandler {
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
        var requestPath = httpContext.Request.Path;
        var userId = httpContext.User?.Identity?.Name ?? "Anonymous";
        var isDevelopment = _environment.IsDevelopment();

        var responseModel = new Response { Succeeded = false };
        int statusCode;

        switch (exception) {
            case UnauthorizedAccessException e:
            _logger.LogWarning(e, "Unauthorized access attempt - Path: {Path}, User: {User}", requestPath, userId);
            responseModel.Message = exception.Message;
            responseModel.StatusCode = HttpStatusCode.Unauthorized;
            statusCode = (int)HttpStatusCode.Unauthorized;
            break;

            case ValidationException e:
            _logger.LogWarning(e, "Validation error - Path: {Path}, User: {User}, Errors: {Errors}",
                requestPath, userId, string.Join(", ", e.Errors.Select(err => err.ErrorMessage)));
            responseModel.Message = exception.Message;
            responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
            responseModel.Errors = [.. e.Errors.Select(err => err.ErrorMessage)];
            statusCode = (int)HttpStatusCode.UnprocessableEntity;
            break;

            case KeyNotFoundException e:
            _logger.LogWarning(e, "Resource not found - Path: {Path}, User: {User}", requestPath, userId);
            responseModel.Message = exception.Message;
            responseModel.StatusCode = HttpStatusCode.NotFound;
            statusCode = (int)HttpStatusCode.NotFound;
            break;

            case DbUpdateException e:
            _logger.LogError(e, "Database update error - Path: {Path}, User: {User}, InnerException: {InnerException}",
                requestPath, userId, e.InnerException?.Message);
            responseModel.Message = isDevelopment
                ? $"Database update failed: {e.InnerException?.Message ?? e.Message}"
                : "An error occurred while updating the database. Please try again.";
            responseModel.StatusCode = HttpStatusCode.BadRequest;
            statusCode = (int)HttpStatusCode.BadRequest;
            break;

            case SqlException e:
            _logger.LogError(e, "SQL error - Path: {Path}, User: {User}, ErrorNumber: {ErrorNumber}, Message: {Message}",
                requestPath, userId, e.Number, e.Message);
            responseModel.Message = isDevelopment ?
                 $"Database error (Code: {e.Number}): {e.Message}"
                : "Somethins went wrong. Please contact support if the problem persists.";
            responseModel.StatusCode = HttpStatusCode.InternalServerError;
            statusCode = (int)HttpStatusCode.InternalServerError;
            break;

            case Exception e when e.GetType().ToString() == "ApiException":
            _logger.LogError(e, "API error - Path: {Path}, User: {User}", requestPath, userId);
            responseModel.Message = isDevelopment
                ? exception.Message + (e.InnerException == null ? "" : "\n" + e.InnerException.Message)
                : "An error occurred while processing your request.";
            responseModel.StatusCode = HttpStatusCode.BadRequest;
            statusCode = (int)HttpStatusCode.BadRequest;
            break;

            default:
            _logger.LogError(exception, "Unhandled exception - Path: {Path}, User: {User}, Type: {ExceptionType}, Message: {Message}, StackTrace: {StackTrace}",
                requestPath, userId, exception?.GetType().Name, exception?.Message, exception?.StackTrace);
            responseModel.Message = isDevelopment
                ? exception?.Message + (exception?.InnerException == null ? "" : "\n" + exception.InnerException.Message) ?? "Something went wrong"
                : "An unexpected error occurred. Please try again later.";
            responseModel.StatusCode = HttpStatusCode.InternalServerError;
            statusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

        return true; // Exception handled
    }
}
