using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// samp-servers-api request data contract class
    /// </summary>
    [DataContract]
    internal class SAMPServersAPIRequestDataContract
    {
        /// <summary>
        /// Banner URI
        /// </summary>
        [DataMember(Name = "banner")]
        private string bannerURI;

        /// <summary>
        /// Core
        /// </summary>
        [DataMember]
        private SAMPServersAPIRequestCoreDataContract core;

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        private string description;

        /// <summary>
        /// Active
        /// </summary>
        [DataMember(Name = "active")]
        private bool isActive = default;

        /// <summary>
        /// Rules
        /// </summary>
        [DataMember(Name = "ru")]
        private Dictionary<string, string> rules;

        /// <summary>
        /// Is active
        /// </summary>
        public bool active => active;

        /// <summary>
        /// Banner
        /// </summary>
        public string BannerURI
        {
            get
            {
                if (bannerURI == null)
                {
                    bannerURI = string.Empty;
                }
                return bannerURI;
            }
        }

        /// <summary>
        /// Core
        /// </summary>
        public SAMPServersAPIRequestCoreDataContract Core
        {
            get
            {
                if (core == null)
                {
                    core = new SAMPServersAPIRequestCoreDataContract();
                }
                return core;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                if (description == null)
                {
                    description = string.Empty;
                }
                return description;
            }
        }

        /// <summary>
        /// Active
        /// </summary>
        public bool IsActive => isActive;

        /// <summary>
        /// Rules
        /// </summary>
        public IReadOnlyDictionary<string, string> Rules
        {
            get
            {
                if (rules == null)
                {
                    rules = new Dictionary<string, string>();
                }
                return rules;
            }
        }
    }
}
