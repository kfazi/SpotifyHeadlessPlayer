namespace HeadlessPlayer
{
    public interface ISpotifySettings
    {
        string Username { get; }

        string Password { get; }

        string DefaultPlaylist { get; }
    }
}