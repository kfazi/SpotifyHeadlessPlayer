namespace HeadlessPlayer
{
    using HeadlessPlayer.Commands;
    using HeadlessPlayer.Events;

    public interface IPlayerStateMachine
    {
        PlayerState PlayerState { get; }

        void FireCommand(ICommand trigger);

        void FireEvent(IEvent trigger);
    }
}