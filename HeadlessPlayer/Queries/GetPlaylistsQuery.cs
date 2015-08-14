namespace HeadlessPlayer.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SpotifySharp;

    public class GetPlaylistsQuery : IQuery<IDictionary<int, string>>
    {
        private readonly ISpotifySession _spotifySession;

        public GetPlaylistsQuery(ISpotifySession spotifySession)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");

            _spotifySession = spotifySession;
        }

        public async Task<IDictionary<int, string>> ExecuteAsync()
        {
            var playlistContainer = _spotifySession.Playlistcontainer();

            await playlistContainer.Load();

            var playlists = new Dictionary<int, string>();
            for (var i = 0; i < playlistContainer.NumPlaylists(); i++)
            {
                if (playlistContainer.PlaylistType(i) != PlaylistType.Playlist)
                {
                    continue;
                }

                var playlist = playlistContainer.Playlist(i);
                await playlist.Load();

                playlists.Add(i, playlist.Name());
            }

            return playlists;
        }
    }
}
