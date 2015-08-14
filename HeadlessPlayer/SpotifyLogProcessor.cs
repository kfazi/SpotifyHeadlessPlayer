namespace HeadlessPlayer
{
    using System.Text.RegularExpressions;

    using NLog;

    public class SpotifyLogProcessor : ISpotifyLogProcessor
    {
        private static readonly Logger SpotifyLog = LogManager.GetLogger("Spotify");

        private readonly Regex _logRegex;

        public SpotifyLogProcessor()
        {
            _logRegex = new Regex(@"(?<time>[\d:.]+)\s+(?<level>\w)\s+(?<file>\[.*?\])\s+(?<message>.*)", RegexOptions.Compiled);
        }

        public void Log(string data)
        {
            var trimmedData = data.Trim('\n', '\r');
            if (string.IsNullOrWhiteSpace(trimmedData))
            {
                return;
            }

            var match = _logRegex.Match(trimmedData);
            if (!match.Success)
            {
                SpotifyLog.Debug(trimmedData);
            }

            var level = match.Groups["level"].Value.ToUpperInvariant();
            var message = match.Groups["message"].Value;
            switch (level)
            {
                case "D":
                    SpotifyLog.Debug(message);
                    break;
                case "I":
                    SpotifyLog.Info(message);
                    break;
                case "W":
                    SpotifyLog.Warn(message);
                    break;
                case "E":
                    SpotifyLog.Error(message);
                    break;
                case "F":
                    SpotifyLog.Fatal(message);
                    break;
                default:
                    SpotifyLog.Debug("{0} - {1}", level, message);
                    break;
            }
        }
    }
}