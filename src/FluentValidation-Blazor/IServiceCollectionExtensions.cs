using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentValidation
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection, params Type[] assemblyTypeMarkers) =>
            AddFluentValidation(serviceCollection, assemblyTypeMarkers, ServiceLifetime.Transient);

        public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection, IEnumerable<Type> assemblyTypeMarkers, ServiceLifetime serviceLifetime) =>
            AddFluentValidation(serviceCollection, assemblyTypeMarkers.Select(x => x.Assembly), serviceLifetime);

        public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection, params Assembly[] assemblies) =>
            AddFluentValidation(serviceCollection, assemblies, ServiceLifetime.Transient);

        public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
        {
            serviceCollection.AddValidatorsFromAssemblies(assemblies, serviceLifetime);
            serviceCollection.AddSingleton<ValidatorLocator>();
            return serviceCollection;
        }
    }
}
