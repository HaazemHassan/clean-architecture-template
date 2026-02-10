using Microsoft.AspNetCore.Identity;
using Template.API.DataSeeding;
using Template.API.Middlewares;
using Template.Core.Entities.IdentityEntities;

namespace Template.API {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();
            builder.Services.AddServices(builder.Configuration);


            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {


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

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandling();
            app.UseForwardedHeaders();   // Use Forwarded Headers (must be early in pipeline)
            app.UseSecurityHeaders();
            app.UseHttpsRedirection();
            app.UseGuestSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();    //must be after UseAuthentication and UseAuthorization be cause we are using user identity name in rate limiting policy
            app.MapControllers();

            app.Run();
        }
    }
}
