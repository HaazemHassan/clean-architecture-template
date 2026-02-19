using ErrorOr;
using Template.Domain.Common.Enums;

namespace Template.Application.Security.Contracts
{
    public interface IAuthorizationService
    {
        ErrorOr<Success> AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies);
    }
}
