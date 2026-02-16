using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Options;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Infrastructure.Data;
using Template.Infrastructure.Data.IdentityEntities;
using Template.Infrastructure.Jobs;
using Template.Infrastructure.Repositories;
using Template.Infrastructure.Services;

namespace Template.Infrastructure;

public static class InfrastructureServiceRegistration {

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {

        AddDbContextConfiguations(services, configuration);
        AddIdentityConfigurations(services, configuration);
        AddRepositories(services);
        AddServices(services);
        AddBackgroundJobs(services);

        return services;
    }


    private static void AddDbContextConfiguations(IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        });

    }


    private static IServiceCollection AddIdentityConfigurations(IServiceCollection services, IConfiguration configuration) {

        var passwordSettings = new PasswordSettings();
        configuration.GetSection(PasswordSettings.SectionName).Bind(passwordSettings);
        services.AddSingleton(passwordSettings);

        services.AddIdentity<ApplicationUser, ApplicationRole>(option => {
            // Password settings 
            option.Password.RequireDigit = passwordSettings.RequireDigit;
            option.Password.RequireLowercase = passwordSettings.RequireLowercase;
            option.Password.RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric;
            option.Password.RequireUppercase = passwordSettings.RequireUppercase;
            option.Password.RequiredLength = passwordSettings.MinLength;
            option.Password.RequiredUniqueChars = passwordSettings.RequiredUniqueChars;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = false;


        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        return services;
    }




    private static IServiceCollection AddRepositories(IServiceCollection services) {

        // UnitOfWork should be Scoped to maintain consistency across a single request
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();



        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services) {
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<ISeederService, SeederService>();


        return services;
    }

    private static IServiceCollection AddBackgroundJobs(IServiceCollection services) {
        services.AddScoped<RefreshTokensCleanupJob>();

        return services;
    }
}
