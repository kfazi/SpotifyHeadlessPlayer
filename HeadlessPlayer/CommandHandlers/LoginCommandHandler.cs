namespace HeadlessPlayer.CommandHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class LoginCommandHandler : IHandlesCommand<LoginCommand>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ISpotifySession _spotifySession;

        public LoginCommandHandler(ISpotifySession spotifySession)
        {
            if (spotifySession == null) throw new ArgumentNullException("spotifySession");

            _spotifySession = spotifySession;
        }

        public async Task HandleAsync(LoginCommand command)
        {
            Log.Info("Processing LoginCommand");

            _spotifySession.Login(command.Username, command.Password, true, null);
        }
    }
}