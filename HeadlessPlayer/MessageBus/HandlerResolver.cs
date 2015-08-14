namespace HeadlessPlayer.MessageBus
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    public class HandlerResolver : IHandlerResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public HandlerResolver(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException("lifetimeScope");

            _lifetimeScope = lifetimeScope;
        }

        public void Resolve()
        {
            _lifetimeScope.Resolve<IEnumerable<IHandler>>();
        }
    }
}