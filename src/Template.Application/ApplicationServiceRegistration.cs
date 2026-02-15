using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.Application.Behaviors;

namespace Template.Application {
    public static class ApplicationServiceRegistration {
        public static IServiceCollection AddApplication(this IServiceCollection services) {

            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddBehaviours(services);
            AddDomainServices(services);

            return services;
        }



        public static void AddBehaviours(IServiceCollection services) {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TrimmingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddDomainServices(IServiceCollection services) {
        }
    }
}