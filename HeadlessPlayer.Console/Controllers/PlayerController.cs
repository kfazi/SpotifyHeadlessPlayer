namespace HeadlessPlayer.Console.Controllers
{
    using System;
    using System.Web.Http;

    using HeadlessPlayer.Commands;

    public class PlayerController : ApiController
    {
        private readonly IPlayer _player;

        public PlayerController(IPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");

            _player = player;
        }

        [Route("play")]
        [HttpGet]
        public void Play()
        {
            _player.SendCommand(new PlayCommand());
        }

        [Route("pause")]
        [HttpGet]
        public void Pause()
        {
            _player.SendCommand(new PauseCommand());
        }

        [Route("stop")]
        [HttpGet]
        public void Stop()
        {
            _player.SendCommand(new StopCommand());
        }

        [Route("next")]
        [HttpGet]
        public void Next()
        {
            _player.SendCommand(new NextSongCommand());
        }

        [Route("previous")]
        [HttpGet]
        public void Previous()
        {
            _player.SendCommand(new PreviousSongCommand());
        }
        
        [Route("playlist/{playlistName}")]
        [HttpGet]
        public void Playlist(string playlistName)
        {
            _player.SendCommand(new SetActivePlaylistCommand { PlaylistName = playlistName });
        }

        [Route("shuffle")]
        [HttpGet]
        public void Shuffle()
        {
            _player.SendCommand(new ShufflePlaylistCommand());
        }
    }
}