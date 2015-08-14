namespace HeadlessPlayer
{
    using System.Threading;
    using System.Threading.Tasks;

    using SpotifySharp;

    public static class TrackExtensions
    {
        public static async Task Load(this ITrack track)
        {
            await Load(track, CancellationToken.None);
        }

        public static async Task Load(this ITrack track, CancellationToken cancellationToken)
        {
            await Task.Run(() => WaitForIsLoaded(track, cancellationToken), cancellationToken);
        }

        private static void WaitForIsLoaded(ITrack track, CancellationToken cancellationToken)
        {
            while (!track.IsLoaded() || cancellationToken.IsCancellationRequested)
            {
                Task.Delay(250, cancellationToken);
            }
        }
    }
}