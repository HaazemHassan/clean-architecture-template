using ErrorOr;
using Template.Application.Contracts.Api;
using Template.Application.Security.Markers;
using Template.Domain.Common.Constants;
using Template.Domain.Common.Enums;

namespace Template.Application.Security.Policies
{
    internal class SelfOrAdminPolicy : IAuthorizationPolicy
    {
        private readonly ICurrentUserService _currentUserService;

        public string Name => AuthorizationPolicies.SelfOrAdmin;

        public SelfOrAdminPolicy(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public ErrorOr<Success> Authorize(object request)
        {
            if (request is not IOwnedResourceRequest ownedRequest)
            {
                return Error.Validation(
                    code: ErrorCodes.Authorization.NotAllowed,
                    description: "Forbidden");
            }


            if (!_currentUserService.IsAuthenticated)

            {
                return Error.Unauthorized(code: ErrorCodes.Authorization.NotAuthenticated, description: "User not authenticated");
            }

            var isAdmin = _currentUserService.IsInRole(UserRole.Admin);


            if (isAdmin || ownedRequest.OwnerUserId == _currentUserService.UserId)
            {
                return Result.Success;
            }

            return Error.Forbidden(code: ErrorCodes.Authorization.NotAllowed, description: "You are not allowed to perform this action on this resource.");


        }
    }

}
