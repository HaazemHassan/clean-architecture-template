using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Template.Core.Entities.IdentityEntities;
using Template.Core.Entities.UserEntities;

namespace Template.API.DataSeeding {
    public static class UserSeeder {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager) {
            int usersInDb = await userManager.Users.CountAsync();
            if (usersInDb > 0)
                return;

            string usersJson = await File.ReadAllTextAsync("DataSeeding/Users.json");
            List<UserSeedData>? seedData = JsonSerializer.Deserialize<List<UserSeedData>>(usersJson);

            if (seedData is null || seedData.Count == 0)
                return;

            foreach (var data in seedData) {

                var domainUser = DomainUser.Create(data.FirstName, data.LastName, data.Email, data.PhoneNumber, data.Address);

                var applicationUser = ApplicationUser.Create(data.Email, data.PhoneNumber);
                applicationUser.AssignDomainUser(domainUser);
                applicationUser.ConfirmEmail();


                var result = await userManager.CreateAsync(applicationUser, data.Password!);

                if (result.Succeeded) {
                    await userManager.AddToRoleAsync(applicationUser, data.Role!);
                }
            }
        }

        private class UserSeedData {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string? Address { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }
        }
    }
}
