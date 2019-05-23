namespace SteamLauncher.CommandConfiguration
{
    using CommandLine;

    /// <summary>
    /// The configure app options.
    /// </summary>
    [Verb(name: "add", HelpText = "Add new account to the configuration.", Hidden = false)]
    public class AddParams
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        [Option('u', "user", Required = true, HelpText = "The name of a user.")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        [Option('s', "secret", Required = true, HelpText = "Accounts secret word.")]
        public string Secret { get; set; }
    }
}