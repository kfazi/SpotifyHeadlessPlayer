namespace HeadlessPlayer.Console
{
    using Westwind.Utilities.Configuration;

    internal class AppSettings : AppConfiguration, IAppSettings
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string DefaultPlaylist { get; set; }

        public string BaseAddress { get; set; }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var provider = new ConfigurationFileConfigurationProvider<AppSettings>();
            return provider;
        }
    }
}