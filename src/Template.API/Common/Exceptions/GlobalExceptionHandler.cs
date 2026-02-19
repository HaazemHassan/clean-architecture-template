using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;

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
        var traceId = httpContext.TraceIdentifier;

        // 1. هنجهز قائمة الأخطاء اللي هنملاها بناءً على نوع الـ Exception
        var errors = new List<object>();
        int statusCode;
        string title;

        // 2. معالجة كل Exception بالـ Logging بتاعه والـ Details
        switch (exception)
        {
            case UnauthorizedException e:
            _logger.LogWarning(e, "Unauthorized - Path: {Path}, User: {User}, TraceId: {TraceId}", requestPath, userId, traceId);
            statusCode = StatusCodes.Status401Unauthorized;
            title = "Unauthorized Access";
            errors.Add(new { Code = "Auth.Unauthorized", Description = e.Message });
            break;

            case ForbiddenException e:
            _logger.LogWarning(e, "Forbidden - Path: {Path}, User: {User}, TraceId: {TraceId}", requestPath, userId, traceId);
            statusCode = StatusCodes.Status403Forbidden;
            title = "Forbidden Action";
            errors.Add(new { Code = "Auth.Forbidden", Description = "You do not have permission to perform this action." });
            break;

            case ValidationException e:
            _logger.LogWarning(e, "Validation error - Path: {Path}, User: {User}", requestPath, userId);
            statusCode = StatusCodes.Status400BadRequest;
            title = "Validation Failed";
            errors.AddRange(e.Errors.Select(v => new { Code = v.PropertyName, Description = v.ErrorMessage }));
            break;

            case KeyNotFoundException e:
            _logger.LogWarning(e, "Not Found - Path: {Path}, User: {User}", requestPath, userId);
            statusCode = StatusCodes.Status404NotFound;
            title = "Resource Not Found";
            errors.Add(new { Code = "Resource.NotFound", Description = e.Message });
            break;

            case SqlException e:
            case DbUpdateException:
            _logger.LogError(exception, "Database error - Path: {Path}, User: {User}, TraceId: {TraceId}", requestPath, userId, traceId);
            statusCode = StatusCodes.Status500InternalServerError;
            title = "Database Error";
            errors.Add(new
            {
                Code = "Database.Error",
                Description = isDevelopment ? exception.Message : "A database error occurred."
            });
            break;

            default:
            _logger.LogError(exception, "Unhandled exception - Path: {Path}, User: {User}, TraceId: {TraceId}", requestPath, userId, traceId);
            statusCode = StatusCodes.Status500InternalServerError;
            title = "Unexpected Error";
            errors.Add(new
            {
                Code = "Server.Error",
                Description = isDevelopment ? exception.Message : "An unexpected error occurred."
            });
            break;
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = requestPath,
            Detail = isDevelopment ? exception.ToString() : null
        };

        problemDetails.Extensions["errors"] = errors;
        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}