namespace steamLauncher.Logic.Services
{
    using System;
    using System.IO;
    using System.Security;

    using steamLauncher.Logic.AppConfiguration;
    using steamLauncher.Logic.Helpers;

    /// <summary>
    /// The configuration service.
    /// </summary>
    public class ConfigurationService
    {
        private SecureString key;

        public ConfigurationService()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public SecureString Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// The read configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="ConfigData"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public ConfigData ReadConfiguration()
        {
            try
            {
                return XmlSerializationHelper.Deserialize<ConfigData>(EnvirenmentHelper.ConfigFilePath);
            }
            catch (Exception exception)
            {
                if (exception is FileNotFoundException || exception is ApplicationException)
                {
                    throw;
                }

                throw new ApplicationException("Unexpected error occured.", exception);
            }
        }

        /// <summary>
        /// The save configuration.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void SaveConfiguration(ConfigData data)
        {
            try
            {
                XmlSerializationHelper.Serialize(EnvirenmentHelper.ConfigFilePath, data);
            }
            catch (Exception exception)
            {
                if (exception is FileNotFoundException || exception is ApplicationException)
                {
                    throw;
                }

                throw new ApplicationException("Unexpected error occured.", exception);
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        private void Initialize()
        {
            if (!File.Exists(EnvirenmentHelper.KeyFilePath))
            {
                var newKey = EncryptionHelper.RandomString(36, true);
                File.WriteAllText(EnvirenmentHelper.KeyFilePath, newKey);
            }
            else
            {
                var rawKey = File.ReadAllText(EnvirenmentHelper.KeyFilePath);
                this.key = new SecureString();
                foreach (var character in rawKey)
                {
                    this.key.AppendChar(character);
                }
            }

            if (!File.Exists(EnvirenmentHelper.ConfigFilePath))
            {
                if (!Directory.Exists(EnvirenmentHelper.AppDataDirectory))
                {
                    Directory.CreateDirectory(EnvirenmentHelper.AppDataDirectory);
                }

                var config = new ConfigData();
                this.SaveConfiguration(config);
            }
        }
    }
}