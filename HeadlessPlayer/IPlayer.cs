namespace HeadlessPlayer
{
    using System;

    using HeadlessPlayer.Commands;

    public interface IPlayer : IDisposable
    {
        void Run(ISpotifySettings spotifySettings);

        void SendCommand(ICommand command);
    }
}