namespace SteamLauncher.Services
{
    public class SteamService
    {
        private readonly ConfigurationService configService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamService"/> class.
        /// </summary>
        /// <param name="configService">
        /// The config service.
        /// </param>
        public SteamService(ConfigurationService configService)
        {
            this.configService = configService;
        }
    }
}