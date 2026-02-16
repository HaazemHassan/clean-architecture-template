using Serilog;
using Template.API.Extentions;
using Template.API.Middlewares;

namespace Template.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDependencies(builder.Configuration);
            builder.Host.UseSerilog((hostingContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostingContext.Configuration);
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                await app.InitializeDatabaseAsync();

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();
            app.UseSerilogRequestLogging();
            app.UseForwardedHeaders();   // Use Forwarded Headers (must be early in pipeline)
            app.UseSecurityHeaders();
            app.UseHttpsRedirection();

            app.UseCustomHangfireDashboard();
            app.RegisterRecurringJobs();
            app.UseCors();

            app.UseGuestSession();
            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsProduction())
            {
                app.UseRateLimiter();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
