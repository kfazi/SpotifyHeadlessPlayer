namespace HeadlessPlayer.EventHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.Events;
    using HeadlessPlayer.MessageBus;
    using HeadlessPlayer.Queries;

    using NLog;

    using SpotifySharp;

    public class LoggedInEventHandler : IHandlesEvent<LoggedInEvent>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly IBus _bus;

        private readonly ISession _session;

        public LoggedInEventHandler(ISpotifySession spotifySession, IBus bus, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (bus == null) throw new ArgumentNullException("bus");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _bus = bus;
            _session = session;
        }

        public async Task HandleAsync(LoggedInEvent message)
        {
            Log.Debug("Processing LoggedInEvent");

            _session.PlayerState = PlayerState.Stopped;
        }
    }
}
