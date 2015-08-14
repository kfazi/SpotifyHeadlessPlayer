namespace HeadlessPlayer
{
    using System.Threading;
    using System.Threading.Tasks;

    using SpotifySharp;

    public static class PlaylistContainerExtensions
    {
        public static async Task Load(this IPlaylistContainer playlistContainer)
        {
            await Load(playlistContainer, CancellationToken.None);
        }

        public static async Task Load(this IPlaylistContainer playlistContainer, CancellationToken cancellationToken)
        {
            await Task.Run(() => WaitForIsLoaded(playlistContainer, cancellationToken), cancellationToken);
        }

        private static void WaitForIsLoaded(IPlaylistContainer playlistContainer, CancellationToken cancellationToken)
        {
            while (!playlistContainer.IsLoaded() || cancellationToken.IsCancellationRequested)
            {
                Task.Delay(250, cancellationToken);
            }
        }
    }
}