namespace HeadlessPlayer.Tests
{
    using NUnit.Framework;

    using SpotifySharp;

    [TestFixture]
    public class SpotifySessionFactoryTests
    {
        private class DummyListener : SpotifySessionListener
        {
        }

        [Test]
        public void ShouldCreateSession()
        {
            var dummyListener = new DummyListener();
            var spotifySessionFactory = new SpotifySessionFactory();

            var session = spotifySessionFactory.CreateSession(dummyListener);

            Assert.IsNotNull(session);
        }
    }
}