namespace SteamLauncher.CommandConfiguration
{
    using CommandLine;

    /// <summary>
    /// The config params.
    /// </summary>
    [Verb("config", HelpText = "Configures required parameters of the application.")]
    public class ConfigParams
    {
        /// <summary>
        /// Gets or sets the steam path.
        /// </summary>
        [Option('s', "steam", HelpText = "The full path to a steam client.", Required = true)]
        public string SteamPath { get; set; }
    }
}