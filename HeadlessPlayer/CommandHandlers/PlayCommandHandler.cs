namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class PlayCommandHandler : IHandlesCommand<PlayCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly ISoundOutput _soundOutput;

        private readonly ISession _session;

        public PlayCommandHandler(ISpotifySession spotifySession, ISoundOutput soundOutput, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (soundOutput == null) throw new ArgumentNullException("soundOutput");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _soundOutput = soundOutput;
            _session = session;
        }

        public async Task HandleAsync(PlayCommand command)
        {
            Log.Info("Processing PlayCommand");

            var playlistPlayStrategy = _session.PlaylistPlayStrategy;

            var track = playlistPlayStrategy.CurrentTrack;

            await track.Load();

            if (_session.PlayerState != PlayerState.Paused)
            {
                LoadTrack(track);
            }

            _soundOutput.Play();

            _session.PlayerState = PlayerState.Playing;
        }

        private void LoadTrack(ITrack track)
        {
            _spotifySession.PlayerLoad(track);

            _spotifySession.PlayerPlay(true);
        }
    }
}