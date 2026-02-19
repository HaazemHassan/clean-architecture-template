using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.Application.Common.Behaviors;
using Template.Application.Common.Behaviors.Transaction;
using Template.Application.Common.Behaviors.Trimming;
using Template.Application.Security.Policies;

namespace Template.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddBehaviours(services);
            AddAuthorizationPolicies(services);
            AddDomainServices(services);

            return services;
        }



        private static void AddBehaviours(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TrimmingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        }


        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationPolicy, SelfOrAdminPolicy>();
            services.AddScoped<IAuthorizationPolicy, SelfOnlyPolicy>();

        }



        private static void AddDomainServices(IServiceCollection services)
        {
        }
    }
}