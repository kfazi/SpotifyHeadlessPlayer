namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    public class ShufflePlaylistCommandHandler : IHandlesCommand<ShufflePlaylistCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISession _session;

        private readonly Random _random;

        public ShufflePlaylistCommandHandler(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            _session = session;

            _random = new Random();
        }

        public async Task HandleAsync(ShufflePlaylistCommand command)
        {
            Log.Info("Processing ShufflePlaylistCommand");

            var playlistPlayStrategy = _session.PlaylistPlayStrategy;
            
            playlistPlayStrategy.OrderPlaylist(x => _random.Next());
        }
    }
}