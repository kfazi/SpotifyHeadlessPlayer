namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    public class PauseCommandHandler : IHandlesCommand<PauseCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISoundOutput _soundOutput;

        private readonly ISession _session;

        public PauseCommandHandler(ISoundOutput soundOutput, ISession session)
        {
            if (soundOutput == null) throw new ArgumentNullException("soundOutput");
            if (session == null) throw new ArgumentNullException("session");

            _soundOutput = soundOutput;
            _session = session;
        }

        public async Task HandleAsync(PauseCommand command)
        {
            Log.Info("Processing PauseCommand");

            _soundOutput.Pause();

            _session.PlayerState = PlayerState.Paused;
        }
    }
}