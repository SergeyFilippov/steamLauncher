namespace SteamLauncher.CommandConfiguration
{
    using CommandLine;

    /// <summary>
    /// The delete params.
    /// </summary>
    [Verb("delete", HelpText = "Deletes specified account from storage. Excepts user name or user number.")]
    public class DeleteParams
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        [Option('u', "user", HelpText = "User name of the account.")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        [Option('n', "number", HelpText = "User given number to be user.")]
        public int Number { get; set; }
    }
}