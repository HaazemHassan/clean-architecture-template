using Microsoft.EntityFrameworkCore;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Enums;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Services
{
    internal class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        //private readonly IMemoryCache _cache;

        public PermissionService(AppDbContext context /*,IMemoryCache cache*/)
        {
            _context = context;
            //_cache = cache;
        }

        //public async Task<bool> HasPermissionsAsync(
        //    string userId,
        //    IEnumerable<Permission> requiredPermissions,
        //    CancellationToken cancellationToken)
        //{
        //    var cacheKey = $"permissions:{userId}";

        //    var userPermissions = await _cache.GetOrCreateAsync(
        //        cacheKey,
        //        async entry =>
        //        {
        //            entry.AbsoluteExpirationRelativeToNow =
        //                TimeSpan.FromMinutes(10);

        //            return await _context.UserRoles
        //                .Where(ur => ur.UserId == userId)
        //                .SelectMany(ur => ur.Role.RolePermissions)
        //                .Select(rp => rp.Permission)
        //                .ToListAsync(cancellationToken);
        //        });

        //    return requiredPermissions.All(p => userPermissions.Contains(p));
        //}

        public async Task<bool> HasPermissionsAsync(int userId, IEnumerable<Permission> requiredPermissions, CancellationToken cancellationToken)
        {
            var required = requiredPermissions.ToList();

            if (!required.Any())
                return true;

            var userPermissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.Permission
                )
                .Distinct().ToListAsync(cancellationToken);

            return required.All(p => userPermissions.Contains(p));
        }
    }

}
