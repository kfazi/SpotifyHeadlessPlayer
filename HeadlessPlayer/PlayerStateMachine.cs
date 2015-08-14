namespace HeadlessPlayer
{
    using System;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.Events;

    using Stateless;

    internal class PlayerStateMachine : IPlayerStateMachine
    {
        private readonly StateMachine<PlayerState, Type> _stateMachine;

        public PlayerStateMachine()
        {
            _stateMachine = new StateMachine<PlayerState, Type>(PlayerState.LoggedOut);

            InitializeRoutes();
        }

        public PlayerState PlayerState
        {
            get
            {
                return _stateMachine.State;
            }
        }

        public void FireCommand(ICommand trigger)
        {
            _stateMachine.Fire(trigger.GetType());
        }

        public void FireEvent(IEvent trigger)
        {
            _stateMachine.Fire(trigger.GetType());
        }

        private void InitializeRoutes()
        {
            _stateMachine.Configure(PlayerState.LoggedOut)
                .Permit(typeof(LoginCommand), PlayerState.LoggingIn);

            _stateMachine.Configure(PlayerState.LoggingIn)
                .Permit(typeof(LoggedInEvent), PlayerState.LoggedIn)
                .Permit(typeof(LoggedOutEvent), PlayerState.LoggedOut)
                .Permit(typeof(LoginFailedEvent), PlayerState.LoggedOut);

            _stateMachine.Configure(PlayerState.LoggedIn)
                .Permit(typeof(StopCommand), PlayerState.Stopped)
                .Permit(typeof(LoggedOutEvent), PlayerState.LoggedOut);

            _stateMachine.Configure(PlayerState.Stopped)
                .Permit(typeof(LoggedOutEvent), PlayerState.LoggedOut)
                .Permit(typeof(PlayCommand), PlayerState.Playing)
                .Permit(typeof(NextSongCommand), PlayerState.Stopped)
                .Permit(typeof(PreviousSongCommand), PlayerState.Stopped);

            _stateMachine.Configure(PlayerState.Playing)
                .Permit(typeof(LoggedOutEvent), PlayerState.LoggedOut)
                .Permit(typeof(StopCommand), PlayerState.Stopped)
                .Permit(typeof(PauseCommand), PlayerState.Paused)
                .Permit(typeof(NextSongCommand), PlayerState.Playing)
                .Permit(typeof(PreviousSongCommand), PlayerState.Playing);

            _stateMachine.Configure(PlayerState.Paused)
                .Permit(typeof(LoggedOutEvent), PlayerState.LoggedOut)
                .Permit(typeof(StopCommand), PlayerState.Stopped)
                .Permit(typeof(PlayCommand), PlayerState.Playing)
                .Permit(typeof(NextSongCommand), PlayerState.Stopped)
                .Permit(typeof(PreviousSongCommand), PlayerState.Stopped);
        }
    }
}