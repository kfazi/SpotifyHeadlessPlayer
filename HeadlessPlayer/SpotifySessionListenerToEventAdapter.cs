namespace HeadlessPlayer
{
    using System;

    using HeadlessPlayer.Events;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class SpotifySessionListenerToEventAdapter : ISpotifySessionListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IBus _bus;

        private readonly ISoundOutput _soundOutput;

        private readonly ISpotifyLogProcessor _spotifyLogProcessor;

        public SpotifySessionListenerToEventAdapter(IBus bus, ISoundOutput soundOutput, ISpotifyLogProcessor spotifyLogProcessor)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            if (soundOutput == null) throw new ArgumentNullException("soundOutput");
            if (spotifyLogProcessor == null) throw new ArgumentNullException("spotifyLogProcessor");

            _bus = bus;
            _soundOutput = soundOutput;
            _spotifyLogProcessor = spotifyLogProcessor;
        }

        public void LoggedIn(ISpotifySession session, SpotifyError error)
        {
            Log.Debug("LoggedIn");

            if (error != SpotifyError.Ok)
            {
                _bus.PublishAsync(new LoginFailedEvent());
                return;
            }

            _bus.PublishAsync(new LoggedInEvent());
        }

        public void LoggedOut(ISpotifySession session)
        {
            Log.Debug("LoggedOut");

            _bus.PublishAsync(new LoggedOutEvent());
        }

        public void MetadataUpdated(ISpotifySession session)
        {
            Log.Debug("MetadataUpdated");
        }

        public void ConnectionError(ISpotifySession session, SpotifyError error)
        {
            Log.Debug("ConnectionError");
        }

        public void MessageToUser(ISpotifySession session, string message)
        {
            Log.Debug("MessageToUser");
        }

        public void NotifyMainThread(ISpotifySession session)
        {
            Log.Debug("NotifyMainThread");
            _bus.PublishAsync(new NotifyMainThreadEvent());
        }

        public int MusicDelivery(ISpotifySession session, AudioFormat format, IntPtr frames, int num_frames)
        {
            if (num_frames == 0)
            {
                return 0;
            }

            _soundOutput.AddSamples(frames, num_frames);

            return num_frames;
        }

        public void PlayTokenLost(ISpotifySession session)
        {
            Log.Debug("PlayTokenLost");
        }

        public void LogMessage(ISpotifySession session, string data)
        {
            _spotifyLogProcessor.Log(data);
        }

        public void EndOfTrack(ISpotifySession session)
        {
            Log.Debug("EndOfTrack");
            _bus.PublishAsync(new TrackFinishedEvent());
        }

        public void StreamingError(ISpotifySession session, SpotifyError error)
        {
            Log.Debug("StreamingError");
        }

        public void UserinfoUpdated(ISpotifySession session)
        {
            Log.Debug("UserinfoUpdated");
        }

        public void StartPlayback(ISpotifySession session)
        {
            Log.Debug("StartPlayback");
        }

        public void StopPlayback(ISpotifySession session)
        {
            Log.Debug("StopPlayback");
        }

        public void GetAudioBufferStats(ISpotifySession session, out AudioBufferStats stats)
        {
            stats.samples = _soundOutput.GetSamplesInBufferCount() / 2;
            stats.stutter = 0;
        }

        public void OfflineStatusUpdated(ISpotifySession session)
        {
            Log.Debug("OfflineStatusUpdated");
        }

        public void OfflineError(ISpotifySession session, SpotifyError error)
        {
            Log.Debug("OfflineError");
        }

        public void CredentialsBlobUpdated(ISpotifySession session, string blob)
        {
            Log.Debug("CredentialsBlobUpdated");
        }

        public void ConnectionstateUpdated(ISpotifySession session)
        {
            Log.Debug("ConnectionstateUpdated");
        }

        public void ScrobbleError(ISpotifySession session, SpotifyError error)
        {
            Log.Debug("ScrobbleError");
        }

        public void PrivateSessionModeChanged(ISpotifySession session, bool is_private)
        {
            Log.Debug("PrivateSessionModeChanged");
        }
    }
}