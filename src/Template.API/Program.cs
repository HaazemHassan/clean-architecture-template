using Template.API.Extentions;
using Template.API.Middlewares;

namespace Template.API {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddServices(builder.Configuration);


            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                await app.InitializeDatabaseAsync();

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandling();
            app.UseForwardedHeaders();   // Use Forwarded Headers (must be early in pipeline)
            app.UseSecurityHeaders();
            app.UseHttpsRedirection();

            app.UseCustomHangfireDashboard();
            app.RegisterRecurringJobs();

            app.UseGuestSession();
            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsProduction()) {
                app.UseRateLimiter();    //must be after UseAuthentication and UseAuthorization be cause we are using user identity name in rate limiting policy
            }

            app.MapControllers();

            app.Run();
        }
    }
}
