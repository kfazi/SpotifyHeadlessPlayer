namespace HeadlessPlayer.Console
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Autofac;

    using HeadlessPlayer.Commands;

    using Microsoft.Owin.Hosting;
    using Microsoft.Owin.Hosting.Services;
    using Microsoft.Owin.Hosting.Starter;

    using NLog;

    public class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var iocBootstrap = new IocBootstrap();

            using (var container = iocBootstrap.Install())
            {
                using (var lifetimeScope = container.BeginLifetimeScope())
                {
                    var appSettings = lifetimeScope.Resolve<IAppSettings>();

                    if (args.Any())
                    {
                        var command = string.Join(" ", args);
                        var result = TrySendCommandAsync(appSettings.BaseAddress, command).Result;
                        if (result)
                        {
                            return;
                        }
                    }

                    Log.Info("Starting new instance...");

                    var player = lifetimeScope.Resolve<IPlayer>();
                    player.Run(appSettings);

                    player.SendCommand(new LoginCommand { Username = appSettings.Username, Password = appSettings.Password });

                    var services = (ServiceProvider)ServicesFactory.Create();
                    var options = new StartOptions(appSettings.BaseAddress);

                    services.AddInstance<ILifetimeScope>(lifetimeScope);

                    var starter = services.GetService<IHostingStarter>();

                    using (starter.Start(options))
                    {
                        if (args.Any())
                        {
                            var command = string.Join(" ", args);
                            TrySendCommandAsync(appSettings.BaseAddress, command).Wait();
                        }

                        Log.Info("Waiting for input to quit");
                        Console.ReadKey(true);
                    }
                }
            }
        }

        private static async Task<bool> TrySendCommandAsync(string baseAddress, string command)
        {
            var client = new HttpClient();

            try
            {
                await client.GetAsync(baseAddress + command);

                Log.Info("Command has been sent to another instance");

                return true;
            }
            catch (HttpRequestException httpRequestException)
            {
                var webException = httpRequestException.InnerException as WebException;
                if (webException == null)
                {
                    throw;
                }

                var socketException = webException.InnerException as SocketException;
                if (socketException != null && socketException.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
