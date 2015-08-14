namespace HeadlessPlayer
{
    using System.Linq;
    using System.Reflection;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;

    using HeadlessPlayer.MessageBus;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterHandlersFromAssembly(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IHandlesCommand<>)
                                                                           || x.GetGenericTypeDefinition() == typeof(IHandlesEvent<>))));
        }
    }
}