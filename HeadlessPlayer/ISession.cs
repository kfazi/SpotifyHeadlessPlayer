namespace HeadlessPlayer
{
    public interface ISession
    {
        IPlaylistPlayStrategy PlaylistPlayStrategy { get; set; }

        PlayerState PlayerState { get; set; }
    }
}