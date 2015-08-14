namespace HeadlessPlayer.MessageBus
{
    using System.Threading.Tasks;

    using HeadlessPlayer.Events;

    public interface IHandlesEvent<in T> : IHandler where T : IEvent
    {
        Task HandleAsync(T message);
    }
}