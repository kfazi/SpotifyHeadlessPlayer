namespace HeadlessPlayer.MessageBus
{
    using System.Linq;
    using System.Threading.Tasks;

    using Autofac;
    using Autofac.Core;

    public class BusAutoregistrationAutofacModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Activated += OnComponentActivated;
        }

        private static void OnComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            if (e == null) return;

            AutoSubscribeHandlers(e.Instance, e.Context);
        }

        private static void AutoSubscribeHandlers(object instance, IComponentContext componentContext)
        {
            if (!(instance is IHandler))
            {
                return;
            }

            var instanceType = instance.GetType();
            var interfaces = instanceType.GetInterfaces();

            var bus = componentContext.Resolve<IBus>();
            foreach (var handlerInterface in interfaces.Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IHandlesCommand<>)
                                                                                       || x.GetGenericTypeDefinition() == typeof(IHandlesEvent<>))))
            {
                var genericType = handlerInterface.GetGenericArguments().First();
                var handleMethodInfo = handlerInterface.GetMethod("HandleAsync");
                bus.Subscribe(
                    genericType,
                    message => (Task)handleMethodInfo.Invoke(instance, new[] { message }));
            }
        }
    }
}