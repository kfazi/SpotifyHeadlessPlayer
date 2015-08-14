namespace HeadlessPlayer.Tests
{
    using Moq;

    using NUnit.Framework;

    using SpotifySharp;

    [TestFixture]
    public class PlayerTests
    {
        private Mock<ISpotifySettings> _spotifySettingsMock;

        private Mock<ISpotifySession> _spotifySessionMock;

        private Player _player;

        [SetUp]
        public void Setup()
        {
            _spotifySettingsMock = new Mock<ISpotifySettings>();
            _spotifySessionMock = new Mock<ISpotifySession>();

            _spotifySettingsMock.Setup(x => x.Username).Returns("Spotify Username");
            _spotifySettingsMock.Setup(x => x.Password).Returns("Spotify Password");

            _player = new Player(_spotifySettingsMock.Object, _spotifySessionMock.Object);
        }

        [Test]
        public void ShouldLoginToSpotify()
        {
            _player.Login();

            _spotifySessionMock.Verify(x => x.Login("Spotify Username", "Spotify Password", true, null), Times.Once);
        }

        [Test]
        public void ShouldPlay()
        {
            _player.Play();

            _spotifySessionMock.Verify(x => x.PlayerPlay(true));
        }

        [Test]
        public void ShouldPause()
        {
            _player.Pause();

            _spotifySessionMock.Verify(x => x.PlayerPlay(false));
        }

        [Test]
        public void ShouldPlayNextTrackWhenTrackFinished()
        {
            _player.EndOfTrack(_spotifySessionMock.Object);
        }
    }
}
