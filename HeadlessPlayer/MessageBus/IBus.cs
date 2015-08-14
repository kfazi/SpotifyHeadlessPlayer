namespace HeadlessPlayer.MessageBus
{
    using System;
    using System.Threading.Tasks;

    public interface IBus
    {
        Task PublishAsync<T>(T message) where T : class;

        IDisposable Subscribe<T>(Func<T, Task> onMessage) where T : class;

        IDisposable Subscribe(Type messageType, Func<object, Task> onMessage);
    }
}