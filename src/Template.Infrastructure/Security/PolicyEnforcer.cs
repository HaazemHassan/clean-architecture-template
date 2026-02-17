using Template.Application.Common.Responses;
using Template.Application.Common.Security;
using Template.Application.Contracts.Requests;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Enums;
using Template.Domain.Enums;

namespace Template.Infrastructure.Security
{
    internal class PolicyEnforcer(ICurrentUserService _currentUserService) : IPolicyEnforcer
    {
        public ServiceOperationResult Authorize<TRequest>(
            TRequest request, int userId, string policyName)
        {
            return policyName switch
            {
                AuthorizationPolicies.SelfOrAdmin
                    when request is IOwnedResourceRequest owned
                        => SelfOrAdminPolicy(owned, userId),

                AuthorizationPolicies.SelfOrAdmin
                        => ServiceOperationResult.Failure(
                                ServiceOperationStatus.InvalidParameters,
                                "Request must implement IOwnedResourceRequest"),

                _ => ServiceOperationResult.Failure(
                        ServiceOperationStatus.Failed,
                        $"Unknown policy: {policyName}")
            };
        }

        private ServiceOperationResult SelfOrAdminPolicy(IOwnedResourceRequest request, int currentUserId)
        {

            if (!_currentUserService.IsAuthenticated)

            {
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "User not authenticated");
            }

            var isAdmin = _currentUserService.IsInRole(UserRole.Admin);


            if (isAdmin || request.OwnerUserId == currentUserId)
            {
                return ServiceOperationResult.Success();
            }

            return ServiceOperationResult.Failure(ServiceOperationStatus.Forbidden, "You are not allowed to perform this action on this resource.");
        }
    }
}
