namespace HeadlessPlayer.MessageBus
{
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;

    public interface IHandlesCommand<in T> : IHandler where T : ICommand
    {
        Task HandleAsync(T command);
    }
}