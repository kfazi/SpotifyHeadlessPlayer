namespace HeadlessPlayer
{
    public class Session : ISession
    {
        public Session()
        {
            PlaylistPlayStrategy = new MemorizedPlaylistPlayStrategy();
            PlayerState = PlayerState.Stopped;
        }

        public IPlaylistPlayStrategy PlaylistPlayStrategy { get; set; }

        public PlayerState PlayerState { get; set; }
    }
}