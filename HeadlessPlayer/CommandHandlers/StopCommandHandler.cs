namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class StopCommandHandler : IHandlesCommand<StopCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly ISoundOutput _soundOutput;

        private readonly ISession _session;

        public StopCommandHandler(ISpotifySession spotifySession, ISoundOutput soundOutput, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (soundOutput == null) throw new ArgumentNullException("soundOutput");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _soundOutput = soundOutput;
            _session = session;
        }

        public async Task HandleAsync(StopCommand command)
        {
            Log.Info("Processing SetActivePlaylistCommand");

            _soundOutput.Stop();

            _spotifySession.PlayerPlay(false);

            _session.PlayerState = PlayerState.Stopped;
        }
    }
}