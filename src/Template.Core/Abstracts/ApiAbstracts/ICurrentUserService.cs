using Template.Core.Enums;

namespace Template.Core.Abstracts.ApiAbstracts;

public interface ICurrentUserService {
    int? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    IList<UserRole> GetRoles();
    bool IsInRole(UserRole roleName);
}