using ErrorOr;
using Template.Application.Contracts.Api;
using Template.Application.Security.Markers;
using Template.Domain.Common.Constants;

namespace Template.Application.Security.Policies
{
    internal class SelfOnlyPolicy(ICurrentUserService _currentUserService) : IAuthorizationPolicy
    {
        public string Name => AuthorizationPolicies.SelfOnly;

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



            if (ownedRequest.OwnerUserId == _currentUserService.UserId)
            {
                return Result.Success;
            }

            return Error.Forbidden(code: ErrorCodes.Authorization.NotAllowed, description: "You are not allowed to perform this action on this resource.");
        }
    }
}
