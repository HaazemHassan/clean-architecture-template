using Template.Domain.Enums;

namespace Template.Application.Contracts.Services.Infrastructure
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionsAsync(int userId, IEnumerable<Permission> requiredPermissions, CancellationToken cancellationToken);
    }
}
