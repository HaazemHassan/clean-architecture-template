using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Identity;
using Template.API.DataSeeding;
using Template.Infrastructure.Common.Options;
using Template.Infrastructure.Data.IdentityEntities;
using Template.Infrastructure.Extensions;

namespace Template.API.Extentions {
    public static class WebApplicationExtensions {

        public static async Task InitializeDatabaseAsync(this WebApplication app) {
            #region Initialize Database
            using (var scope = app.Services.CreateScope()) {

                /*
                // needed to dockerize the application and have the DB created automatically
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (app.Environment.IsDevelopment()) {
                    try {
                        await context.Database.EnsureCreatedAsync();
                        await context.Database.MigrateAsync();
                    }
                    catch (SqlException ex) when (ex.Number == 2714) {
                        Console.WriteLine("Tables already exist, skipping migration");
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Database migration error: {ex.Message}");
                        throw;
                    }
                }
                //
                */

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                await ApplicationRoleSeeder.SeedAsync(roleManager);
                await UserSeeder.SeedAsync(userManager);

            }
            #endregion
        }

        public static void UseCustomHangfireDashboard(this WebApplication app) {
            var hangfireSettings = app.Services.GetRequiredService<HangfireSettings>();
            app.UseHangfireDashboard(hangfireSettings.DashboardPath, new DashboardOptions {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = hangfireSettings.Username,
                        Pass = hangfireSettings.Password
                    }
                ]
            });
        }

        public static void RegisterRecurringJobs(this WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                jobManager.RegisterRecurringJobs();
            }
        }
    }
}
