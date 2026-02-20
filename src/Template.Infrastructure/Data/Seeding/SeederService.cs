using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Application.Contracts.Infrastructure;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Domain.Common.Enums;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Identity.Entities;

namespace Template.Infrastructure.Data.Seeding
{
    internal class SeederService : ISeederService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;

        public SeederService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork,
            AppDbContext context,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = context;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task SeedRolesAsync(List<RoleSeedDto> rolesSeedData, CancellationToken cancellationToken = default)
        {
            if (rolesSeedData is null || rolesSeedData.Count == 0)
                return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            foreach (var roleData in rolesSeedData)
            {
                if (string.IsNullOrWhiteSpace(roleData.Name))
                    continue;

                var role = await _roleManager.FindByNameAsync(roleData.Name);

                if (role is null)
                {
                    role = new ApplicationRole(roleData.Name);

                    var createResult = await _roleManager.CreateAsync(role);

                    if (!createResult.Succeeded)
                        continue;
                }

                if (roleData.Permissions?.Count > 0)
                {
                    await SyncRolePermissionsAsync(
                        role.Id,
                        roleData.Permissions,
                        cancellationToken);
                }
            }



            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        private async Task SyncRolePermissionsAsync(int roleId, List<string> permissionStrings, CancellationToken cancellationToken)
        {
            var validPermissions = new HashSet<Permission>();

            foreach (var permissionString in permissionStrings)
            {
                if (Enum.TryParse<Permission>(permissionString, ignoreCase: true, out var parsedPermission))
                {
                    validPermissions.Add(parsedPermission);
                }
            }


            var existingPermissions = await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToHashSetAsync(cancellationToken);

            var permissionsToAdd = validPermissions
                .Where(p => !existingPermissions.Contains(p))
                .Select(p => new RolePermission
                {
                    RoleId = roleId,
                    Permission = p
                })
                .ToList();

            var permissionsToRemove = await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId && !validPermissions.Contains(rp.Permission))
                .ToListAsync(cancellationToken);

            if (permissionsToAdd.Count > 0)
                await _context.Set<RolePermission>().AddRangeAsync(
                    permissionsToAdd,
                    cancellationToken);

            if (permissionsToRemove.Count > 0)
                _context.Set<RolePermission>().RemoveRange(permissionsToRemove);
        }

        public async Task SeedUsersAsync(List<UserSeedDto> usersSeedData, CancellationToken cancellationToken = default)
        {
            if (usersSeedData is null || usersSeedData.Count == 0)
                return;

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var userData in usersSeedData)
                {
                    var existingUser = await _userManager.FindByEmailAsync(userData.Email);
                    if (existingUser is not null)
                        continue;

                    var normalizedPhone =
                        _phoneNumberService.Normalize(userData.PhoneNumber);

                    var domainUser = new DomainUser(
                        userData.FirstName,
                        userData.LastName,
                        userData.Email,
                        normalizedPhone,
                        userData.Address
                    );

                    var result = await _applicationUserService
                        .AddUser(domainUser, userData.Password);

                    if (result.IsError)
                        throw new Exception(
                            $"Failed to create user {userData.Email}: {result.FirstError.Description}");

                    var createdUser = await _userManager.FindByEmailAsync(userData.Email);

                    if (createdUser is not null)
                    {
                        createdUser.EmailConfirmed = true;
                        await _userManager.UpdateAsync(createdUser);
                    }
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }
    }
}
