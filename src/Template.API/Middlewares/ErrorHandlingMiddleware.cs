using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Template.Core.Bases.Responses;

namespace Template.API.Middlewares {
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment environment) {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
        private readonly IHostEnvironment _environment = environment;

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            catch (Exception error) {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false };
                
                var requestPath = context.Request.Path;
                var userId = context.User?.Identity?.Name ?? "Anonymous";
                var isDevelopment = _environment.IsDevelopment();
                
                //TODO:: cover all validation errors
                switch (error) {
                    case UnauthorizedAccessException e:
                    // custom application error
                    _logger.LogWarning(e, "Unauthorized access attempt - Path: {Path}, User: {User}", requestPath, userId);
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                    case ValidationException e:
                    // custom validation error
                    _logger.LogWarning(e, "Validation error - Path: {Path}, User: {User}, Errors: {Errors}", requestPath, userId, string.Join(", ", e.Errors.Select(err => err.ErrorMessage)));
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    responseModel.Errors = [.. e.Errors.Select(err => err.ErrorMessage)];
                    break;
                    case KeyNotFoundException e:
                    // not found error
                    _logger.LogWarning(e, "Resource not found - Path: {Path}, User: {User}", requestPath, userId);
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                    case DbUpdateException e:
                    // Database update error - Log detailed info but send generic message to client
                    _logger.LogError(e, "Database update error - Path: {Path}, User: {User}, InnerException: {InnerException}", 
                        requestPath, userId, e.InnerException?.Message);
                    responseModel.Message = isDevelopment 
                        ? $"Database update failed: {e.InnerException?.Message ?? e.Message}"
                        : "An error occurred while updating the database. Please try again.";
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                    case SqlException e:
                    // SQL error - Log all details including error number, but send generic message
                    _logger.LogError(e, "SQL error - Path: {Path}, User: {User}, ErrorNumber: {ErrorNumber}, Message: {Message}", 
                        requestPath, userId, e.Number, e.Message);
                    responseModel.Message = isDevelopment 
                        ? $"Database error (Code: {e.Number}): {e.Message}"
                        : "A database error occurred. Please contact support if the problem persists.";
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                    case Exception e when e.GetType().ToString() == "ApiException":
                    // API exception
                    _logger.LogError(e, "API error - Path: {Path}, User: {User}", requestPath, userId);
                    responseModel.Message = isDevelopment 
                        ? error.Message + (e.InnerException == null ? "" : "\n" + e.InnerException.Message)
                        : "An error occurred while processing your request.";
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                  
                    default:
                    // Unhandled error - Log everything but send generic message to client
                    _logger.LogError(error, "Unhandled exception - Path: {Path}, User: {User}, Type: {ExceptionType}, Message: {Message}, StackTrace: {StackTrace}", 
                        requestPath, userId, error?.GetType().Name, error?.Message, error?.StackTrace);
                    responseModel.Message = isDevelopment 
                        ? error?.Message + (error?.InnerException == null ? "" : "\n" + error.InnerException.Message) ?? "Something went wrong"
                        : "An unexpected error occurred. Please try again later.";
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
