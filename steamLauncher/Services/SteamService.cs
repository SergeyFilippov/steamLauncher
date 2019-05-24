namespace SteamLauncher.Services
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using SteamLauncher.AppConfiguration;
    using SteamLauncher.Helpers;

    public class SteamService
    {
        private readonly ConfigurationService configService;

        private readonly UserService userService;

        private readonly ConfigData data;

        private const string argumentsTemplate = "-login {0} {1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamService"/> class.
        /// </summary>
        /// <param name="configService">
        ///     The config service.
        /// </param>
        /// <param name="userService"></param>
        public SteamService(ConfigurationService configService, UserService userService)
        {
            this.configService = configService;
            this.userService = userService;
            this.data = this.configService.ReadConfiguration();
        }

        /// <summary>
        /// The run for account.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void RunForAccount(int id)
        {
            var current = this.userService.GetUser(id);
            this.RunForAccount(current);
        }

        /// <summary>
        /// The run for account.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void RunForAccount(string user)
        {
            var current = this.userService.GetUser(user);
            this.RunForAccount(current);
        }

        /// <summary>
        /// The run for account.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void RunForAccount(UserData account)
        {
            if (this.IsSteamRunning())
            {
                throw new ApplicationException("Steam is already running. Please turn it off before starting new.");
            }

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = this.data.SteamPath;

            var rawSecret = EncryptionHelper.Decrypt(this.configService.Key, account.Secret);
            startInfo.Arguments = string.Format(argumentsTemplate, account.User, rawSecret);

            Console.WriteLine("Starting Steam for " + account.User);

            Process.Start(startInfo);
        }

        /// <summary>
        /// The is steam running.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsSteamRunning()
        {
            return Process.GetProcesses().Any(
                item => item.ProcessName.Equals("steam.exe", StringComparison.InvariantCultureIgnoreCase)
                        || item.ProcessName.Equals("steam", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}