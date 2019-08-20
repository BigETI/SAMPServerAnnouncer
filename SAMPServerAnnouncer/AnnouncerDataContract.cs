using System.Net;
using System.Runtime.Serialization;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// Announcer data contract class
    /// </summary>
    [DataContract]
    internal class AnnouncereDataContract
    {
        /// <summary>
        /// API
        /// </summary>
        [DataMember]
        private string api = default;

        /// <summary>
        /// API value
        /// </summary>
        private EAnnouncerAPI? apiValue;

        /// <summary>
        /// Host
        /// </summary>
        [DataMember(IsRequired = false)]
        private string host;

        /// <summary>
        /// Method
        /// </summary>
        [DataMember(IsRequired = false)]
        private string method;

        /// <summary>
        /// Port
        /// </summary>
        [DataMember(IsRequired = false)]
        private ushort port;

        /// <summary>
        /// Referer
        /// </summary>
        [DataMember(IsRequired = false)]
        private string referer;

        /// <summary>
        /// Use custom server info
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = false)]
        private bool useCustomServerInfo = true;

        /// <summary>
        /// Use HTTPS
        /// </summary>
        [DataMember(IsRequired = false)]
        private bool useHTTPS = default;

        /// <summary>
        /// User agent
        /// </summary>
        [DataMember(IsRequired = false)]
        private string userAgent;

        /// <summary>
        /// Version
        /// </summary>
        [DataMember(IsRequired = false)]
        private string version;

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid => ((Host.Length > 0) && (Port > 0) && (UserAgent.Length > 0));

        /// <summary>
        /// API
        /// </summary>
        public EAnnouncerAPI API
        {
            get
            {
                if (apiValue == null)
                {
                    apiValue = EAnnouncerAPI.Unknown;
                    if (api != null)
                    {
                        switch (api.Trim().ToLower())
                        {
                            case "legacy":
                                apiValue = EAnnouncerAPI.Legacy;
                                break;
                            case "samp-servers-api":
                            case "sampservers-api":
                            case "samp-serversapi":
                            case "sampserversapi":
                                apiValue = EAnnouncerAPI.SAMPServersAPI;
                                break;
                        }
                    }
                }
                return apiValue.Value;
            }
        }

        /// <summary>
        /// Host
        /// </summary>
        public string Host
        {
            get
            {
                if (string.IsNullOrWhiteSpace(host))
                {
                    switch (API)
                    {
                        case EAnnouncerAPI.Legacy:
                            host = "server.sa-mp.com";
                            break;
                        case EAnnouncerAPI.SAMPServersAPI:
                            host = "api.samp-servers.net";
                            break;
                        default:
                            host = string.Empty;
                            break;
                    }
                }
                return host.Trim();
            }
        }

        /// <summary>
        /// Method
        /// </summary>
        public string Method
        {
            get
            {
                if (string.IsNullOrWhiteSpace(method))
                {
                    method = ((API == EAnnouncerAPI.SAMPServersAPI) ? WebRequestMethods.Http.Post : WebRequestMethods.Http.Get);
                }
                return method;
            }
        }

        /// <summary>
        /// Port
        /// </summary>
        public ushort Port
        {
            get => port;
            set => port = value;
        }

        /// <summary>
        /// Referer
        /// </summary>
        public string Referer
        {
            get
            {
                if (string.IsNullOrWhiteSpace(referer))
                {
                    referer = ((API == EAnnouncerAPI.Legacy) ? "http://Bonus" : string.Empty);
                }
                return referer.Trim();
            }
        }

        /// <summary>
        /// Use custom server info
        /// </summary>
        public bool UseCustomServerInfo => useCustomServerInfo;

        /// <summary>
        /// Use HTTPS
        /// </summary>
        public bool UseHTTPS => useHTTPS;

        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(userAgent))
                {
                    userAgent = ((API == EAnnouncerAPI.Legacy) ? "SAMP/0.30" : "SAMPServerAnnouncer/1.0");
                }
                return userAgent.Trim();
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get
            {
                if (version == null)
                {
                    version = string.Empty;
                }
                return version.Trim();
            }
        }
    }
}
