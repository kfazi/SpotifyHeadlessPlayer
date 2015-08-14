namespace HeadlessPlayer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using HeadlessPlayer.Events;
    using HeadlessPlayer.MessageBus;

    public class PlayerThreadSynchronization : IPlayerThreadSynchronization, IHandlesEvent<NotifyMainThreadEvent>, IDisposable
    {
        private readonly AutoResetEvent _synchronizationEvent;

        public PlayerThreadSynchronization()
        {
            _synchronizationEvent = new AutoResetEvent(true);
        }

        ~PlayerThreadSynchronization()
        {
            Dispose(false);
        }

        public async Task HandleAsync(NotifyMainThreadEvent message)
        {
            Set();
        }

        public void Wait(TimeSpan timeout)
        {
            _synchronizationEvent.WaitOne(timeout);
        }

        public void Set()
        {
            _synchronizationEvent.Set();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _synchronizationEvent.Dispose();
        }
    }
}