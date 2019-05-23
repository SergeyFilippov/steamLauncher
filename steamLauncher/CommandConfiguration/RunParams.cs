namespace SteamLauncher.CommandConfiguration
{
    using CommandLine;

    /// <summary>
    /// The run params.
    /// </summary>
    [Verb("run", HelpText = "Runs the steam instance for specified account.")]
    public class RunParams
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        [Option('u', "user", HelpText = "User name of the account to use for steam login.")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        [Option('n', "number", HelpText = "User given number to be used as login for steam.")]
        public int Number { get; set; }
    }
}