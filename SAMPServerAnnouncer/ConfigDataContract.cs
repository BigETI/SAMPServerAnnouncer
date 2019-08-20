using System;
using System.Runtime.Serialization;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// Configuration data contract class
    /// </summary>
    [DataContract]
    internal class ConfigDataContract
    {
        /// <summary>
        /// Announcers
        /// </summary>
        [DataMember]
        private AnnouncereDataContract[] announcers;

        /// <summary>
        /// Custom server information
        /// </summary>
        [DataMember(IsRequired = false)]
        private SAMPServersAPIRequestDataContract customServerInfo = default;

        /// <summary>
        /// IPv4 address
        /// </summary>
        [DataMember(IsRequired = false)]
        private string ipv4Address;

        /// <summary>
        /// IPv4 service URIs
        /// </summary>
        [DataMember(IsRequired = false)]
        private string[] ipv4ServiceURIs;

        /// <summary>
        /// User agent
        /// </summary>
        [DataMember(IsRequired = false)]
        private string userAgent;

        /// <summary>
        /// IPv4 service URIs
        /// </summary>
        public string[] IPv4ServiceURIs
        {
            get
            {
                if (ipv4ServiceURIs == null)
                {
                    ipv4ServiceURIs = Array.Empty<string>();
                }
                return ipv4ServiceURIs;
            }
        }

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
        /// Announcers
        /// </summary>
        public AnnouncereDataContract[] Announcers
        {
            get
            {
                if (announcers == null)
                {
                    announcers = Array.Empty<AnnouncereDataContract>();
                }
                return announcers;
            }
        }

        /// <summary>
        /// Custom server information
        /// </summary>
        public SAMPServersAPIRequestDataContract CustomServerInfo => customServerInfo;

        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(userAgent))
                {
                    userAgent = "SAMPServerAnnouncer/1.0";
                }
                return userAgent;
            }
        }
    }
}
