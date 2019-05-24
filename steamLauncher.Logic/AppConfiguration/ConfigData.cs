namespace steamLauncher.Logic.AppConfiguration
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// The config data.
    /// </summary>
    [XmlRoot]
    public class ConfigData 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigData"/> class.
        /// </summary>
        public ConfigData()
        {
            this.Accounts = new List<UserData>();
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        [XmlArray("Accounts")]
        [XmlArrayItem("Account")]
        public List<UserData> Accounts { get; set; }


        [XmlAttribute]
        public string SteamPath { get; set; }

        /// <summary>
        /// Gets or sets the total number of users.
        /// </summary>
        [XmlAttribute]
        public int TotalNumberOfUsers
        {
            get
            {
                return this.Accounts.Count;
            }
            set
            {
                return;
            }
        }
    }
}