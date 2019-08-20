using System.Runtime.Serialization;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// samp-servers-api request core data contract class
    /// </summary>
    [DataContract]
    internal class SAMPServersAPIRequestCoreDataContract
    {
        /// <summary>
        /// IPv4 address
        /// </summary>
        [DataMember(Name = "ip")]
        private string ipv4Address;

        /// <summary>
        /// Hostname
        /// </summary>
        [DataMember(Name = "hn")]
        private string hostname;

        /// <summary>
        /// Player count
        /// </summary>
        [DataMember(Name = "pc")]
        private uint playerCount = default;

        /// <summary>
        /// Max players
        /// </summary>
        [DataMember(Name = "pm")]
        private uint maxPlayers = default;

        /// <summary>
        /// Gamemode
        /// </summary>
        [DataMember(Name = "gm")]
        private string gamemode;

        /// <summary>
        /// Language
        /// </summary>
        [DataMember(Name = "la")]
        private string language;

        /// <summary>
        /// Requires password
        /// </summary>
        [DataMember(Name = "pa")]
        private bool requiresPassword = default;

        /// <summary>
        /// Server version
        /// </summary>
        [DataMember(Name = "vn")]
        private string serverVersion;

        /// <summary>
        /// IPv4 address
        /// </summary>
        public string IPv4Address
        {
            get
            {
                if (ipv4Address == null)
                {
                    ipv4Address = string.Empty;
                }
                return ipv4Address;
            }
        }

        /// <summary>
        /// Hostname
        /// </summary>
        public string Hostname
        {
            get
            {
                if (hostname == null)
                {
                    hostname = string.Empty;
                }
                return hostname;
            }
        }

        /// <summary>
        /// Player count
        /// </summary>
        public uint PlayerCount => playerCount;

        /// <summary>
        /// Max players
        /// </summary>
        public uint MaxPlayers => maxPlayers;

        /// <summary>
        /// Gamemode
        /// </summary>
        public string Gamemode
        {
            get
            {
                if (gamemode == null)
                {
                    gamemode = string.Empty;
                }
                return gamemode;
            }
        }

        /// <summary>
        /// Language
        /// </summary>
        public string Language
        {
            get
            {
                if (language == null)
                {
                    language = string.Empty;
                }
                return language;
            }
        }

        /// <summary>
        /// Requires password
        /// </summary>
        public bool RequiresPassword => requiresPassword;

        /// <summary>
        /// Server version
        /// </summary>
        public string ServerVersion
        {
            get
            {
                if (serverVersion == null)
                {
                    serverVersion = string.Empty;
                }
                return serverVersion;
            }
        }
    }
}
