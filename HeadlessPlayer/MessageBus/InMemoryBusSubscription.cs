namespace HeadlessPlayer.MessageBus
{
    using System;
    using System.Collections.Generic;

    public class InMemoryBusSubscription : IDisposable
    {
        private readonly IList<object> _subscription;

        private readonly object _onMessage;

        public InMemoryBusSubscription(IList<object> subscription, object onMessage)
        {
            if (subscription == null) throw new ArgumentNullException("subscription");
            if (onMessage == null) throw new ArgumentNullException("onMessage");

            _subscription = subscription;
            _onMessage = onMessage;
        }

        public void Dispose()
        {
            _subscription.Remove(_onMessage);
        }
    }
}