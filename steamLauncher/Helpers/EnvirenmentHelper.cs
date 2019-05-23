namespace SteamLauncher.Helpers
{
    using System;
    using System.IO;

    public static class EnvirenmentHelper
    {
        private static string applicationDirectory;

        private static string appDataDirectory;

        private static string configFilePath;

        private static string keyFilePath;

        /// <summary>
        /// Gets the application directory.
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                return applicationDirectory ?? (applicationDirectory = AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        /// <summary>
        /// Gets the app data directory.
        /// </summary>
        public static string AppDataDirectory
        {
            get
            {
                return appDataDirectory ?? (appDataDirectory =
                                                Path.Combine(
                                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "steamLauncher"));
            }
        }

        /// <summary>
        /// Gets the config file path.
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                return configFilePath ?? (configFilePath = Path.Combine(AppDataDirectory, "storage.config"));
            }
        }

        /// <summary>
        /// Gets the key file path.
        /// </summary>
        public static string KeyFilePath
        {
            get
            {
                return keyFilePath ?? (keyFilePath = Path.Combine(ApplicationDirectory, "key.config"));
            }
        }
    }
}