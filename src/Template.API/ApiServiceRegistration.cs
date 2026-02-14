using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.RateLimiting;
using Template.API.Authorization;
using Template.API.Authorization.Requirements;
using Template.API.Exceptions;
using Template.API.Filters;
using Template.API.RateLimiting;
using Template.API.Services;
using Template.Application;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Api;
using Template.Infrastructure;
using Template.Infrastructure.Common.Options;




namespace Template.API {
    public static class ApiServiceRegistration {
        private const string GuestIdKey = "GuestId";   // used for ratelimiting

        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration) {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            AddApi(services, configuration);


            return services;


        }


        private static IServiceCollection AddApi(IServiceCollection services, IConfiguration configuration) {
            services.AddControllers();
            AddAuthenticationConfigurations(services, configuration);
            AddSwaggerConfigurations(services);
            AddAutorizationConfigurations(services);
            AddRateLimitingConfigurations(services, configuration);
            AddHangfireConfiguration(services, configuration);

            // Register Exception Handler
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            #region Api Services
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IClientContextService, ClientContextService>();
            #endregion

            return services;
        }



        private static IServiceCollection AddSwaggerConfigurations(IServiceCollection services) {
            // Swagger Configuration
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Template API",
                    Version = "v1",
                    Description = "API for Template application"
                });
                options.OperationFilter<SwaggerExcludeOperationFilter>();

                //options.EnableAnnotations();

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath)) {
                    options.IncludeXmlComments(xmlPath);
                }

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }


        private static IServiceCollection AddAuthenticationConfigurations(IServiceCollection services, IConfiguration configuration) {
            //JWT Authentication
            var jwtSettings = new JwtSettings();
            configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);



            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x => {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters {
                   ValidateIssuer = jwtSettings.ValidateIssuer,
                   ValidIssuer = jwtSettings.Issuer,
                   ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                   ValidAudience = jwtSettings.Audience,
                   ValidateAudience = jwtSettings.ValidateAudience,
                   ValidateLifetime = jwtSettings.ValidateLifeTime,
                   ClockSkew = TimeSpan.FromMinutes(2)
               };


           }
        );

            return services;
        }
        private static IServiceCollection AddAutorizationConfigurations(IServiceCollection services) {
            services.AddAuthorizationBuilder()
                .AddPolicy(AuthorizationPolicies.ResetPassword, policy => {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("purpose", "reset-password");
                })
                .AddPolicy(AuthorizationPolicies.SameUserOrAdmin, policy => {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new SameUserOrAdminRequirement());
                });
            return services;
        }

        public static IServiceCollection AddRateLimitingConfigurations(
         this IServiceCollection services, IConfiguration configuration) {

            var rateLimitingSettings = new RateLimitingSettings();
            configuration.GetSection(RateLimitingSettings.SectionName).Bind(rateLimitingSettings);
            services.AddSingleton(rateLimitingSettings);

            services.AddRateLimiter(options => {
                options.AddPolicy(RateLimitingPolicies.DefaultLimiter, httpContext => {
                    string partitionKey;
                    var user = httpContext.User?.Identity?.Name;

                    if (!string.IsNullOrEmpty(user))
                        partitionKey = user;

                    else if (httpContext.Items.TryGetValue(GuestIdKey, out var guestId) && guestId is string guestIdString)
                        partitionKey = guestIdString;

                    else
                        partitionKey = GetFallbackPartitionKey(httpContext);


                    return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, key => new SlidingWindowRateLimiterOptions {
                        Window = TimeSpan.FromMinutes(rateLimitingSettings.DefaultLimiter.WindowMinutes),
                        PermitLimit = rateLimitingSettings.DefaultLimiter.PermitLimit,
                        QueueLimit = rateLimitingSettings.DefaultLimiter.QueueLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        SegmentsPerWindow = rateLimitingSettings.DefaultLimiter.SegmentsPerWindow
                    });
                });

                options.AddPolicy(RateLimitingPolicies.LoginLimiter, httpContext => {
                    string partitionKey;

                    if (httpContext.Items.TryGetValue(GuestIdKey, out var guestId) && guestId is string guestIdString)
                        partitionKey = guestIdString;

                    else
                        partitionKey = GetFallbackPartitionKey(httpContext);


                    return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, key => new SlidingWindowRateLimiterOptions {
                        Window = TimeSpan.FromMinutes(rateLimitingSettings.LoginLimiter.WindowMinutes),
                        PermitLimit = rateLimitingSettings.LoginLimiter.PermitLimit,
                        QueueLimit = rateLimitingSettings.LoginLimiter.QueueLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        SegmentsPerWindow = rateLimitingSettings.LoginLimiter.SegmentsPerWindow
                    });
                });

                options.OnRejected = async (context, token) => {
                    if (context.HttpContext.Response.HasStarted) {
                        return;
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    int retryAfterSeconds = rateLimitingSettings.RetryAfterSeconds;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)) {
                        retryAfterSeconds = (int)Math.Ceiling(retryAfter.TotalSeconds);
                    }
                    context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString();

                    var response = new Response<string> {
                        StatusCode = HttpStatusCode.TooManyRequests,
                        Message = "Too many requests. Please try again later.",
                        Succeeded = false
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
                };

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;

            static string GetFallbackPartitionKey(HttpContext httpContext) {
                var clientContextService = httpContext.RequestServices.GetRequiredService<IClientContextService>();
                var ip = clientContextService.GetClientIpAddress();
                var userAgent = httpContext.Request.Headers.UserAgent;

                var identifier = $"{ip}-{userAgent}";
                var bytes = Encoding.UTF8.GetBytes(identifier);
                var hash = SHA256.HashData(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private static IServiceCollection AddHangfireConfiguration(IServiceCollection services, IConfiguration configuration) {

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

    }
}

