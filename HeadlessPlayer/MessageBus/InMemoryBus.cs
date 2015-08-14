namespace HeadlessPlayer.MessageBus
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class InMemoryBus : IBus, IDisposable
    {
        private readonly Dictionary<Type, Collection<object>> _subscriptions;

        public InMemoryBus()
        {
            _subscriptions = new Dictionary<Type, Collection<object>>();
        }

        public void Dispose()
        {
            _subscriptions.Clear();
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            var messageType = message.GetType();

            await PublishToSubscriptions(message, messageType);
        }

        public IDisposable Subscribe<T>(Func<T, Task> onMessage) where T : class
        {
            var messageType = typeof(T);
            return Subscribe(messageType, x => onMessage((T)x));
        }

        public IDisposable Subscribe(Type messageType, Func<object, Task> onMessage)
        {
            if (!_subscriptions.ContainsKey(messageType))
            {
                _subscriptions.Add(messageType, new Collection<object>());
            }

            var subscription = _subscriptions[messageType];
            subscription.Add(onMessage);

            return new InMemoryBusSubscription(subscription, onMessage);
        }

        private async Task PublishToSubscriptions<T>(T message, Type messageType) where T : class
        {
            if (!_subscriptions.ContainsKey(messageType))
            {
                return;
            }

            foreach (var subscribeAction in _subscriptions[messageType])
            {
                await ((Func<T, Task>)subscribeAction)(message);
            }
        }
    }
}
