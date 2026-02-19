using Hangfire;
using HangfireBasicAuthenticationFilter;
using System.Text.Json;
using Template.Infrastructure.BackgroundJobs;
using Template.Infrastructure.Data.Seeding;

namespace Template.API.Extentions
{
    public static class WebApplicationExtensions
    {

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            #region Initialize Database
            using (var scope = app.Services.CreateScope())
            {

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

                var logger = app.Services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var seederService = scope.ServiceProvider.GetRequiredService<ISeederService>();

                    string rolesJson = await File.ReadAllTextAsync("DataSeeding/Roles.json");
                    string usersJson = await File.ReadAllTextAsync("DataSeeding/Users.json");

                    List<RoleSeedDto>? rolesSeedData = JsonSerializer.Deserialize<List<RoleSeedDto>>(rolesJson);
                    List<UserSeedDto>? usersSeedData = JsonSerializer.Deserialize<List<UserSeedDto>>(usersJson);

                    await seederService.SeedRolesAsync(rolesSeedData!);
                    await seederService.SeedUsersAsync(usersSeedData!);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;

                }
                #endregion
            }
        }

        public static void UseCustomHangfireDashboard(this WebApplication app)
        {
            var hangfireSettings = app.Services.GetRequiredService<HangfireSettings>();
            app.UseHangfireDashboard(hangfireSettings.DashboardPath, new DashboardOptions
            {
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

        public static void RegisterRecurringJobs(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                jobManager.RegisterRecurringJobs();
            }
        }
    }
}
