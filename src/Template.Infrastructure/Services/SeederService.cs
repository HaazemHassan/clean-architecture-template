using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;
using Template.Infrastructure.Data.IdentityEntities;
using Template.Infrastructure.Data.Seeding;

namespace Template.Infrastructure.Services {
    internal class SeederService : ISeederService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public SeederService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager) {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync(List<string> rolesSeedData, CancellationToken cancellationToken = default) {
            bool haveRoles = await _roleManager.Roles.AnyAsync(cancellationToken);
            if (haveRoles)
                return;

            if (rolesSeedData is null || rolesSeedData.Count <= 0)
                return;

            var normalizedRoles = rolesSeedData.Where(r => !string.IsNullOrWhiteSpace(r))
                                                 .Select(r => r.Trim())
                                                 .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var role in normalizedRoles) {
                var roleInDb = await _roleManager.FindByNameAsync(role);
                if (roleInDb is null) {
                    roleInDb = new ApplicationRole(role);
                    await _roleManager.CreateAsync(roleInDb);
                }
            }
        }

        public async Task SeedUsersAsync(List<UserSeedDto> usersSeedData, CancellationToken cancellationToken = default) {
            bool haveUsers = await _unitOfWork.Users.AnyAsync(cancellationToken);
            if (haveUsers)
                return;


            if (usersSeedData is null || usersSeedData.Count == 0)
                return;

            foreach (var data in usersSeedData) {

                var domainUser = new DomainUser(data.FirstName, data.LastName, data.Email, data.PhoneNumber, data.Address);

                var applicationUser = new ApplicationUser(data.Email, data.PhoneNumber);
                applicationUser.AssignDomainUser(domainUser);
                applicationUser.ConfirmEmail();


                var result = await _userManager.CreateAsync(applicationUser, data.Password!);

                if (result.Succeeded) {
                    await _userManager.AddToRoleAsync(applicationUser, data.Role!);
                }
            }
        }
    }
}
