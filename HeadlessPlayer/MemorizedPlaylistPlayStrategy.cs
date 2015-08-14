namespace HeadlessPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpotifySharp;

    public class MemorizedPlaylistPlayStrategy : IPlaylistPlayStrategy
    {
        private readonly List<ITrack> _nextTracks;

        private readonly List<ITrack> _previousTracks;

        private List<ITrack> _playlist;

        public MemorizedPlaylistPlayStrategy()
        {
            _playlist = new List<ITrack>();
            _nextTracks = new List<ITrack>();
            _previousTracks = new List<ITrack>();
        }

        public IReadOnlyList<ITrack> Playlist
        {
            get
            {
                return _playlist;
            }
        }

        public ITrack CurrentTrack { get; private set; }

        public ITrack Next()
        {
            if (CurrentTrack != null)
            {
                RememberCurrentTrackIn(_previousTracks);
            }

            if (_nextTracks.Any())
            {
                UseRememberedTrackFrom(_nextTracks);
                return CurrentTrack;
            }

            var nextTrackIndex = GetNextTrackIndex();
            CurrentTrack = _playlist[nextTrackIndex];
            
            return CurrentTrack;
        }

        public ITrack Previous()
        {
            if (CurrentTrack != null)
            {
                RememberCurrentTrackIn(_nextTracks);
            }

            if (_previousTracks.Any())
            {
                UseRememberedTrackFrom(_previousTracks);
                return CurrentTrack;
            }

            var previousTrackIndex = GetPreviousTrackIndex();
            CurrentTrack = _playlist[previousTrackIndex];

            return CurrentTrack;
        }

        public void SetPlaylist(IEnumerable<ITrack> playlist)
        {
            _playlist = new List<ITrack>(playlist);
            
            Next();
        }

        public void OrderPlaylist(Func<ITrack, int> orderBy)
        {
            _playlist = _playlist.OrderBy(orderBy).ToList();
        }

        private int GetNextTrackIndex()
        {
            var nextTrackIndex = _playlist.IndexOf(CurrentTrack) + 1;
            if (nextTrackIndex >= _playlist.Count)
            {
                nextTrackIndex = 0;
            }

            return nextTrackIndex;
        }

        private int GetPreviousTrackIndex()
        {
            var previousTrackIndex = _playlist.IndexOf(CurrentTrack) - 1;
            if (previousTrackIndex < 0)
            {
                previousTrackIndex = _playlist.Count - 1;
            }

            return previousTrackIndex;
        }

        private void RememberCurrentTrackIn(IList<ITrack> tracks)
        {
            tracks.Add(CurrentTrack);
            if (tracks.Count > 10000)
            {
                tracks.RemoveAt(0);
            }
        }

        private void UseRememberedTrackFrom(ICollection<ITrack> tracks)
        {
            CurrentTrack = tracks.Last();
            tracks.Remove(CurrentTrack);
        }
    }
}