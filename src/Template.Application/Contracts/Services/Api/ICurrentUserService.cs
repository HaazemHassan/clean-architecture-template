using Template.Domain.Enums;

namespace Template.Application.Contracts.Services.Api;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    IList<UserRole> GetRoles();
    bool IsInRole(UserRole roleName);
    bool HasAllRoles(List<UserRole> requiredRoles);

    IList<Permission> GetPermissions();

    bool HasPermission(Permission permission);
    bool HasAllPermissions(List<Permission> permissions);

}