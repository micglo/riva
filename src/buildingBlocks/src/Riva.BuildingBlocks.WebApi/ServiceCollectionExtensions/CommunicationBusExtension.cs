using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class CommunicationBusExtension
    {
        public static IServiceCollection AddCommunicationBus(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

            services.TryAddTransient<ServiceFactory>(p => p.GetService);
            services.TryAddTransient<ICommunicationBus, CommunicationBus>();
            RegisterCommunicationBusHandlers(services, assemblies);

            return services;
        }

        private static void RegisterCommunicationBusHandlers(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var assembliesToScan = (assemblies as Assembly[] ?? assemblies).Distinct().ToArray();

            ConnectImplementationsToTypesClosing(services, typeof(ICommandHandler<>), assembliesToScan);
            ConnectImplementationsToTypesClosing(services, typeof(IDomainEventHandler<>), assembliesToScan);
            ConnectImplementationsToTypesClosing(services, typeof(IIntegrationEventHandler<>), assembliesToScan);
        }

        private static void ConnectImplementationsToTypesClosing(IServiceCollection services, Type openRequestInterface,
            IEnumerable<Assembly> assembliesToScan)
        {
            var concretions = new List<Type>();
            var interfaces = new List<Type>();
            foreach (var type in assembliesToScan.SelectMany(a => a.DefinedTypes).Where(t => !t.IsOpenGeneric()))
            {
                var interfaceTypes = type.FindInterfacesThatClose(openRequestInterface).ToArray();
                if (!interfaceTypes.Any())
                    continue;

                if (type.IsConcrete())
                    concretions.Add(type);

                foreach (var interfaceType in interfaceTypes)
                {
                    interfaces.Fill(interfaceType);
                }
            }

            foreach (var @interface in interfaces)
            {
                var exactMatches = concretions.Where(x => x.CanBeCastTo(@interface)).ToList();
                if (exactMatches.Count > 1)
                    exactMatches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));

                foreach (var type in exactMatches)
                {
                    services.AddTransient(@interface, type);
                }

                if (!@interface.IsOpenGeneric())
                    AddConcretionsThatCouldBeClosed(@interface, concretions, services);
            }
        }

        private static bool IsMatchingWithInterface(Type handlerType, Type handlerInterface)
        {
            if (handlerType is null || handlerInterface is null)
                return false;

            if (handlerType.IsInterface)
            {
                if (handlerType.GenericTypeArguments.SequenceEqual(handlerInterface.GenericTypeArguments))
                    return true;
            }
            else
            {
                return IsMatchingWithInterface(handlerType.GetInterface(handlerInterface.Name), handlerInterface);
            }

            return false;
        }

        private static void AddConcretionsThatCouldBeClosed(Type interfaceType, IEnumerable<Type> concretions, IServiceCollection services)
        {
            foreach (var type in concretions.Where(x => x.IsOpenGeneric() && x.CouldCloseTo(interfaceType)))
            {
                services.TryAddTransient(interfaceType, type.MakeGenericType(interfaceType.GenericTypeArguments));
            }
        }

        private static bool CouldCloseTo(this Type openConcretion, Type closedInterface)
        {
            var openInterface = closedInterface.GetGenericTypeDefinition();
            var arguments = closedInterface.GenericTypeArguments;

            var concreteArguments = openConcretion.GenericTypeArguments;
            return arguments.Length == concreteArguments.Length && openConcretion.CanBeCastTo(openInterface);
        }

        private static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType is null)
                return false;

            if (pluggedType == pluginType) 
                return true;

            return pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo());
        }

        private static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

        public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            return FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();
        }

        private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType is null)
                yield break;

            if (!pluggedType.IsConcrete())
                yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                foreach (
                    var interfaceType in
                    pluggedType.GetInterfaces()
                        .Where(type => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == templateType))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.GetTypeInfo().BaseType!.GetTypeInfo().IsGenericType &&
                     pluggedType.GetTypeInfo().BaseType?.GetGenericTypeDefinition() == templateType)
            {
                yield return pluggedType.GetTypeInfo().BaseType;
            }

            if (pluggedType.GetTypeInfo().BaseType == typeof(object))
                yield break;

            foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

        private static bool IsConcrete(this Type type)
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }

        private static void Fill<T>(this ICollection<T> list, T value)
        {
            if (list.Contains(value)) 
                return;
            list.Add(value);
        }
    }
}