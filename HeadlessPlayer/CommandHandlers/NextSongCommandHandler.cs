namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class NextSongCommandHandler : IHandlesCommand<NextSongCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly ISession _session;

        public NextSongCommandHandler(ISpotifySession spotifySession, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _session = session;
        }

        public async Task HandleAsync(NextSongCommand message)
        {
            Log.Info("Processing NextSongCommand");

            var playlistPlayStrategy = _session.PlaylistPlayStrategy;

            var track = playlistPlayStrategy.Next();

            await track.Load();

            _spotifySession.PlayerLoad(track);

            if (_session.PlayerState == PlayerState.Playing)
            {
                _spotifySession.PlayerPlay(true);
            }
        }
    }
}