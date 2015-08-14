namespace HeadlessPlayer
{
    using System;
    using System.Threading;

    using Autofac;

    using HeadlessPlayer.Commands;
    using HeadlessPlayer.MessageBus;

    using NLog;

    using SpotifySharp;

    public class Player : IPlayer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Thread _backgroundThread;

        private bool _exitBackgroundThread;

        private ILifetimeScope _lifetimeScope;

        ~Player()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run(ISpotifySettings spotifySettings)
        {
            var iocBootstrap = new IocBootstrap();
            _lifetimeScope = iocBootstrap.Install(spotifySettings);

            var handlerResolver = _lifetimeScope.Resolve<IHandlerResolver>();
            handlerResolver.Resolve();

            var soundOutput = _lifetimeScope.Resolve<ISoundOutput>();
            soundOutput.Volume = 0.1f;
            soundOutput.Play();

            _backgroundThread = new Thread(BackgroundThreadEntryPoint);
            _backgroundThread.IsBackground = true;
            _backgroundThread.Start();
        }

        public void SendCommand(ICommand command)
        {
            var bus = _lifetimeScope.Resolve<IBus>();
            bus.PublishAsync(command).Wait();
        }

        private void BackgroundThreadEntryPoint()
        {
            var spotifySession = _lifetimeScope.Resolve<ISpotifySession>();
            var playerThreadSynchronization = _lifetimeScope.Resolve<IPlayerThreadSynchronization>();

            var nextTimeout = 0;
            while (!_exitBackgroundThread)
            {
                Log.Debug("Next timeout = {0}", nextTimeout);

                playerThreadSynchronization.Wait(TimeSpan.FromMilliseconds(nextTimeout));

                do
                {
                    spotifySession.ProcessEvents(ref nextTimeout);
                }
                while (nextTimeout == 0 && !_exitBackgroundThread);
            }

            spotifySession.PlayerUnload();
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_lifetimeScope != null)
            {
                if (_backgroundThread != null)
                {
                    _exitBackgroundThread = true;

                    var playerThreadSynchronization = _lifetimeScope.Resolve<IPlayerThreadSynchronization>();
                    playerThreadSynchronization.Set();
                    
                    _backgroundThread.Join();
                    _backgroundThread = null;
                }

                _lifetimeScope.Dispose();
                _lifetimeScope = null;
            }
        }
    }
}