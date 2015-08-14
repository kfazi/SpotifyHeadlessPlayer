namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class SetActivePlaylistCommandHandler : IHandlesCommand<SetActivePlaylistCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly ISession _session;

        public SetActivePlaylistCommandHandler(ISpotifySession spotifySession, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _session = session;
        }

        public async Task HandleAsync(SetActivePlaylistCommand command)
        {
            Log.Info("Processing SetActivePlaylistCommand");

            var playlistContainer = _spotifySession.Playlistcontainer();
            
            await playlistContainer.Load();

            var playlist = await FindPlaylistByName(playlistContainer, command.PlaylistName);
            if (playlist == null)
            {
                Log.Warn("Playlist {0} not found", command.PlaylistName);
            }

            var tracks = await GetTracks(playlist);

            var playlistPlayStrategy = _session.PlaylistPlayStrategy;
            playlistPlayStrategy.SetPlaylist(tracks);
        }

        private static async Task<IPlaylist> FindPlaylistByName(IPlaylistContainer playlistContainer, string playlistName)
        {
            for (var i = 0; i < playlistContainer.NumPlaylists(); i++)
            {
                if (playlistContainer.PlaylistType(i) != PlaylistType.Playlist)
                {
                    continue;
                }

                var playlist = playlistContainer.Playlist(i);

                await playlist.Load();

                if (playlist.Name() == playlistName)
                {
                    return playlist;
                }
            }

            return null;
        }

        private static async Task<IEnumerable<ITrack>> GetTracks(IPlaylist playlist)
        {
            var tracks = new List<ITrack>();
            for (var trackIndex = 0; trackIndex < playlist.NumTracks(); trackIndex++)
            {
                var track = playlist.Track(trackIndex);

                await track.Load();

                if (track.IsPlaceholder())
                {
                    continue;
                }

                tracks.Add(track);
            }

            return tracks;
        }
    }
}