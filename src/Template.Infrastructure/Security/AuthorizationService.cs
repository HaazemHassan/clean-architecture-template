using ErrorOr;
using Template.Application.Contracts.Api;
using Template.Application.Security.Contracts;
using Template.Domain.Common.Constants;
using Template.Domain.Common.Enums;

namespace Template.Infrastructure.Security
{
    internal class AuthorizationService(ICurrentUserService _currentUserService, IPolicyEnforcer _policyEnforcer) : IAuthorizationService
    {
        public ErrorOr<Success> AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies
            )
        {


            if (!_currentUserService.HasAllPermissions(requiredPermissions))
            {
                return Error.Forbidden(code: ErrorCodes.Authorization.MissingPermissions, description: "User is missing required permissions for taking this action");
            }

            if (!_currentUserService.HasAllRoles(requiredRoles))
            {
                return Error.Forbidden(code: ErrorCodes.Authorization.MissingRoles, description: "User is missing required roles for taking this action");
            }



            foreach (var policy in requiredPolicies)
            {
                var authorizationAgainstPolicyResult = _policyEnforcer.Authorize(request, policy);

                if (authorizationAgainstPolicyResult.IsError)
                {
                    return authorizationAgainstPolicyResult.Errors;
                }
            }

            return Result.Success;
        }


    }
}
