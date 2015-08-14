namespace HeadlessPlayer
{
    using System.Threading;
    using System.Threading.Tasks;

    using SpotifySharp;

    public static class PlaylistExtensions
    {
        public static async Task Load(this IPlaylist playlist)
        {
            await Load(playlist, CancellationToken.None);
        }

        public static async Task Load(this IPlaylist playlist, CancellationToken cancellationToken)
        {
            await Task.Run(() => WaitForIsLoaded(playlist, cancellationToken), cancellationToken);
        }

        private static void WaitForIsLoaded(IPlaylist playlist, CancellationToken cancellationToken)
        {
            while (!playlist.IsLoaded() || cancellationToken.IsCancellationRequested)
            {
                Task.Delay(250, cancellationToken);
            }
        }
    }
}