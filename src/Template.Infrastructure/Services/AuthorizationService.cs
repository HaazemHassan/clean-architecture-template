using Template.Application.Common.Responses;
using Template.Application.Common.Security;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Enums;
using Template.Domain.Enums;

namespace Template.Infrastructure.Services
{
    internal class AuthorizationService(ICurrentUserService _currentUserService, IPolicyEnforcer _policyEnforcer) : IAuthorizationService
    {
        public ServiceOperationResult AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies
            )
        {

            var userId = _currentUserService.UserId;


            if (!_currentUserService.HasAllPermissions(requiredPermissions))
            {
                return ServiceOperationResult.Failure(ServiceOperationStatus.Forbidden, "User is missing required permissions for taking this action");
            }

            if (!_currentUserService.HasAllRoles(requiredRoles))
            {
                return ServiceOperationResult.Failure(ServiceOperationStatus.Forbidden, "User is missing required roles for taking this action");
            }



            foreach (var policy in requiredPolicies)
            {
                var authorizationAgainstPolicyResult = _policyEnforcer.Authorize(request, userId!.Value, policy);

                if (!authorizationAgainstPolicyResult.Succeeded)
                {
                    return authorizationAgainstPolicyResult;
                }
            }

            return ServiceOperationResult.Success();
        }


    }
}
