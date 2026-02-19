using ErrorOr;
using MediatR;
using System.Reflection;
using Template.Application.Common.Exceptions;
using Template.Application.Security;
using Template.Application.Security.Contracts;
using Template.Application.Security.Markers;
using Template.Domain.Common.Enums;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
IAuthorizationService _authorizationService)
     : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IAuthorizedRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attributes = typeof(TRequest).GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (attributes.Count == 0)
        {
            return await next();

        }

        var requiredPermissions = attributes.SelectMany(a => a.Permissions ?? Array.Empty<Permission>()).ToList();
        var requiredRoles = attributes.SelectMany(a => a.Roles ?? Array.Empty<UserRole>()).ToList();

        var requiredPolicies = attributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
            .Select(a => a.Policy!)
            .ToList();

        var authorizationResult = _authorizationService.AuthorizeCurrentUser(
           request,
           requiredRoles,
           requiredPermissions,
           requiredPolicies);

        if (authorizationResult.IsError)
        {
            var errorType = authorizationResult.FirstError.Type;
            var message = authorizationResult.FirstError.Description;
            throw errorType switch
            {
                ErrorType.Unauthorized => new UnauthorizedException(message),
                ErrorType.Forbidden => new ForbiddenException(message),
                _ => new ForbiddenException(message)
            };
        }

        return await next();
    }
}
