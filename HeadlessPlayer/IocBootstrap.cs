namespace HeadlessPlayer
{
    using Autofac;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using SpotifySharp;

    public class IocBootstrap
    {
        public ILifetimeScope Install(ISpotifySettings spotifySettings)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<BusAutoregistrationAutofacModule>();
            containerBuilder.RegisterHandlersFromAssembly(typeof(ICommand).Assembly).AsImplementedInterfaces();
            containerBuilder.RegisterType<PlayerThreadSynchronization>().As<IPlayerThreadSynchronization>().SingleInstance();
            containerBuilder.RegisterType<InMemoryBus>().As<IBus>().SingleInstance();
            containerBuilder.RegisterType<HandlerResolver>().As<IHandlerResolver>().SingleInstance();
            containerBuilder.RegisterType<Session>().As<ISession>().SingleInstance();
            containerBuilder.RegisterType<SoundOutput>().As<ISoundOutput>().SingleInstance();
            containerBuilder.RegisterType<SpotifyLogProcessor>().As<ISpotifyLogProcessor>().SingleInstance();
            containerBuilder.Register(CreateSpotifySession).As<ISpotifySession>().SingleInstance();
            containerBuilder.RegisterInstance(spotifySettings);

            return containerBuilder.Build();
        }

        private static ISpotifySession CreateSpotifySession(IComponentContext componentContext)
        {
            var spotifySessionFactory = new SpotifySessionFactory(componentContext.Resolve<IBus>(), componentContext.Resolve<ISoundOutput>(), componentContext.Resolve<ISpotifyLogProcessor>());

            return spotifySessionFactory.Create();
        }
    }
}