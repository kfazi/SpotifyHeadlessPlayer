namespace HeadlessPlayer
{
    using System;
    using System.Collections.Generic;

    using SpotifySharp;

    public interface IPlaylistPlayStrategy
    {
        IReadOnlyList<ITrack> Playlist { get; }
        
        ITrack CurrentTrack { get; }

        ITrack Next();
        
        ITrack Previous();

        void SetPlaylist(IEnumerable<ITrack> playlist);

        void OrderPlaylist(Func<ITrack, int> orderBy);
    }
}