using Template.Application.Common.Responses;
using Template.Application.Common.Security;
using Template.Domain.Enums;

namespace Template.Application.Contracts.Services.Infrastructure
{
    public interface IAuthorizationService
    {
        ServiceOperationResult AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies);
    }
}
