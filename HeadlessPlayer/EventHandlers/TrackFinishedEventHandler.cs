namespace HeadlessPlayer.EventHandlers
{
    using System;
    using System.Threading.Tasks;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.Events;
    using HeadlessPlayer.MessageBus;

    using NLog;

    public class TrackFinishedEventHandler : IHandlesEvent<TrackFinishedEvent>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IBus _bus;

        public TrackFinishedEventHandler(IBus bus)
        {
            if (bus == null) throw new ArgumentNullException("bus");

            _bus = bus;
        }

        public async Task HandleAsync(TrackFinishedEvent message)
        {
            Log.Debug("Processing TrackFinishedEvent");

            var nextSongCommand = new NextSongCommand();

            await _bus.PublishAsync(nextSongCommand);
        }
    }
}