namespace HeadlessPlayer.Console
{
    using System;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Owin;

    public class Startup
    {
        private readonly ILifetimeScope _lifetimeScope;

        public Startup(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException("lifetimeScope");

            _lifetimeScope = lifetimeScope;
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var resolver = new AutofacWebApiDependencyResolver(_lifetimeScope);

            var httpConfiguration = new HttpConfiguration { DependencyResolver = resolver };
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Filters.Add(new ExceptionHandlingAttribute());
            httpConfiguration.EnsureInitialized();

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}