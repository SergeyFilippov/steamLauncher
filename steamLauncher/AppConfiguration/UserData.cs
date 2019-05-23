namespace SteamLauncher.AppConfiguration
{
    using System;
    using System.Net.NetworkInformation;

    public class UserData
    {
        public string User { get; set; }

        public string Secret { get; set; }

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}