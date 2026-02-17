using MediatR;
using System.Reflection;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Security;
using Template.Application.Contracts.Requests;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Enums;
using Template.Domain.Enums;

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

        if (!authorizationResult.Succeeded)
        {
            throw authorizationResult.Status switch
            {
                ServiceOperationStatus.Unauthorized => new UnauthorizedException(authorizationResult.Message),
                ServiceOperationStatus.Forbidden => new ForbiddenException(authorizationResult.Message),
                _ => new ForbiddenException(authorizationResult.Message)
            };
        }

        return await next();
    }
}
