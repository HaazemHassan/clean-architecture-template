using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Options;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Security.Contracts;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Infrastructure.BackgroundJobs;
using Template.Infrastructure.BackgroundJobs.Jobs;
using Template.Infrastructure.Data;
using Template.Infrastructure.Data.Identity.Entities;
using Template.Infrastructure.Data.Repositories;
using Template.Infrastructure.Data.Seeding;
using Template.Infrastructure.Security;
using Template.Infrastructure.Services;

namespace Template.Infrastructure;

public static class InfrastructureServiceRegistration
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        AddDbContextConfiguations(services, configuration);
        AddIdentityConfigurations(services, configuration);
        AddRepositories(services);
        AddServices(services);
        AddHangfireConfiguration(services, configuration);
        AddBackgroundJobs(services);

        return services;
    }


    private static void AddDbContextConfiguations(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        });

    }


    private static IServiceCollection AddIdentityConfigurations(IServiceCollection services, IConfiguration configuration)
    {

        var passwordSettings = new PasswordSettings();
        configuration.GetSection(PasswordSettings.SectionName).Bind(passwordSettings);
        services.AddSingleton(passwordSettings);

        services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
        {
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

    private static IServiceCollection AddHangfireConfiguration(IServiceCollection services, IConfiguration configuration)
    {

        var hangfireSettings = new HangfireSettings();
        configuration.GetSection(HangfireSettings.SectionName).Bind(hangfireSettings);
        services.AddSingleton(hangfireSettings);

        services.AddHangfire(config => config
         .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(IServiceCollection services)
    {
        services.AddScoped<RefreshTokensCleanupJob>();

        return services;
    }


    private static IServiceCollection AddRepositories(IServiceCollection services)
    {

        // UnitOfWork should be Scoped to maintain consistency across a single request
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();



        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddTransient<ISeederService, SeederService>();
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPolicyEnforcer, PolicyEnforcer>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddSingleton<IPhoneNumberService, PhoneNumberService>();




        return services;
    }


}
