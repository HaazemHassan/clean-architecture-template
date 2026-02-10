using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Template.Core.Entities.IdentityEntities;

namespace Template.API.DataSeeding {


    public static class ApplicationRoleSeeder {

        public static async Task SeedAsync(RoleManager<ApplicationRole> _roleManager) {

            int rolesCount = await _roleManager.Roles.CountAsync();
            if (rolesCount > 0)
                return;

            string rolesJson = await File.ReadAllTextAsync("DataSeeding/Roles.json");

            List<string>? roles = JsonSerializer.Deserialize<List<string>>(rolesJson);

            if (roles is null || roles.Count <= 0)
                return;

            var normalizedRoles = roles.Where(r => !string.IsNullOrWhiteSpace(r))
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
    }
}