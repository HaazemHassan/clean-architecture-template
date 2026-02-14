using System.Security.Claims;
using Template.Application.Contracts.Services.Api;
using Template.Domain.Enums;

namespace Template.API.Services {
    public class CurrentUserService : ICurrentUserService {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId {
            get {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IList<UserRole> GetRoles() {
            if (!IsAuthenticated)
                return new List<UserRole>();

            var roleClaimsStrings = _httpContextAccessor.HttpContext?.User?
                                    .FindAll(ClaimTypes.Role)
                                    .Select(c => c.Value)
                                    .ToList() ?? new List<string>();

            var rolesList = new List<UserRole>();
            foreach (var roleString in roleClaimsStrings) {
                if (Enum.TryParse<UserRole>(roleString, true, out var roleEnum)) {
                    rolesList.Add(roleEnum);
                }
            }
            return rolesList;
        }

        public bool IsInRole(UserRole roleName) {
            if (!IsAuthenticated)
                return false;

            var roleString = roleName.ToString();

            return _httpContextAccessor.HttpContext?.User?.IsInRole(roleString) ?? false;
        }
    }
}