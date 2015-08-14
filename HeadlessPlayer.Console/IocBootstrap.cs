namespace HeadlessPlayer.Console
{
    using System.Reflection;

    using Autofac;
    using Autofac.Integration.WebApi;

    public class IocBootstrap
    {
        public ILifetimeScope Install()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterType<Player>().As<IPlayer>().SingleInstance();
            containerBuilder.Register(x => ReadConfiguration()).AsSelf().AsImplementedInterfaces().SingleInstance();

            return containerBuilder.Build();
        }

        private static IAppSettings ReadConfiguration()
        {
            var appSettings = new AppSettings();
            appSettings.Initialize();
            return appSettings;
        }
    }
}