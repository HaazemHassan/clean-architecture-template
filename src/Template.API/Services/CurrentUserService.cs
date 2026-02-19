using System.Security.Claims;
using Template.Application.Contracts.Api;
using Template.Domain.Common.Enums;

namespace Template.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IList<UserRole> GetRoles()
        {
            if (!IsAuthenticated)
                return new List<UserRole>();

            var roleClaimsStrings = _httpContextAccessor.HttpContext?.User?
                                    .FindAll(ClaimTypes.Role)
                                    .Select(c => c.Value)
                                    .ToList() ?? new List<string>();

            var rolesList = new List<UserRole>();
            foreach (var roleString in roleClaimsStrings)
            {
                if (Enum.TryParse<UserRole>(roleString, true, out var roleEnum))
                {
                    rolesList.Add(roleEnum);
                }
            }
            return rolesList;
        }

        public bool IsInRole(UserRole roleName)
        {
            if (!IsAuthenticated)
                return false;

            var roleString = roleName.ToString();

            return _httpContextAccessor.HttpContext?.User?.IsInRole(roleString) ?? false;
        }

        public bool HasAllRoles(List<UserRole> requiredRoles)
        {
            if (!IsAuthenticated)
                return false;

            if (requiredRoles.Count == 0)
                return true;

            var roles = GetRoles();
            return requiredRoles.All(r => roles.Contains(r));
        }

        public IList<Permission> GetPermissions()
        {
            if (!IsAuthenticated)
                return new List<Permission>();

            var permissionClaims = _httpContextAccessor.HttpContext?.User?
                                    .FindAll("Permission")
                                    .Select(c => c.Value)
                                    .ToList() ?? new List<string>();

            var permissionsList = new List<Permission>();
            foreach (var permissionString in permissionClaims)
            {
                if (Enum.TryParse<Permission>(permissionString, true, out var permissionEnum))
                {
                    permissionsList.Add(permissionEnum);
                }
                else if (int.TryParse(permissionString, out var permissionInt) &&
                         Enum.IsDefined(typeof(Permission), permissionInt))
                {
                    permissionsList.Add((Permission)permissionInt);
                }
            }
            return permissionsList;
        }

        public bool HasPermission(Permission permission)
        {
            if (!IsAuthenticated)
                return false;

            return GetPermissions().Contains(permission);
        }

        public bool HasAllPermissions(List<Permission> permissions)
        {
            if (!IsAuthenticated)
                return false;

            if (permissions.Count == 0)
                return true;

            var userPermissions = GetPermissions();
            return permissions.All(p => userPermissions.Contains(p));
        }

        public bool HasAnyPermission(params Permission[] permissions)
        {
            if (!IsAuthenticated || permissions.Length == 0)
                return false;

            var userPermissions = GetPermissions();
            return permissions.Any(p => userPermissions.Contains(p));
        }


    }
}