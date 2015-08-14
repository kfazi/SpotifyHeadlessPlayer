namespace HeadlessPlayer.EventHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Events;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class LoggedOutEventHandler : IHandlesEvent<LoggedOutEvent>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        private readonly ISession _session;

        public LoggedOutEventHandler(ISpotifySession spotifySession, ISession session)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");
            if (session == null) throw new ArgumentNullException("session");

            _spotifySession = spotifySession;
            _session = session;
        }

        public async Task HandleAsync(LoggedOutEvent message)
        {
            Log.Debug("Processing LoggedOutEvent");
        }
    }
}