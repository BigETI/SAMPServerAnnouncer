using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// Announcer client class
    /// </summary>
    internal class AnnouncerClient
    {
        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SAMPServersAPIRequestDataContract), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });

        /// <summary>
        /// Host
        /// </summary>
        private string host;

        /// <summary>
        /// MEthod
        /// </summary>
        private string method;

        /// <summary>
        /// IPv4 address
        /// </summary>
        private string ipv4Address;

        /// <summary>
        /// Referer
        /// </summary>
        private string referer;

        /// <summary>
        /// User agent
        /// </summary>
        private string userAgent;

        /// <summary>
        /// Version
        /// </summary>
        private string version;

        /// <summary>
        /// API
        /// </summary>
        public EAnnouncerAPI API { get; private set; }

        /// <summary>
        /// Custom server information
        /// </summary>
        public SAMPServersAPIRequestDataContract CustomServerInfo { get; private set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host
        {
            get
            {
                if (host == null)
                {
                    host = string.Empty;
                }
                return host;
            }
        }

        /// <summary>
        /// Method
        /// </summary>
        public string Method
        {
            get
            {
                if (method == null)
                {
                    method = ((API == EAnnouncerAPI.SAMPServersAPI) ? WebRequestMethods.Http.Post : WebRequestMethods.Http.Get);
                }
                return method;
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
        /// Port
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// Referer
        /// </summary>
        public string Referer
        {
            get
            {
                if (referer == null)
                {
                    referer = string.Empty;
                }
                return referer;
            }
        }

        /// <summary>
        /// Use HTTPS
        /// </summary>
        public bool UseHTTPS { get; private set; }

        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent
        {
            get
            {
                if (userAgent == null)
                {
                    userAgent = string.Empty;
                }
                return userAgent;
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
                return version;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">API</param>
        /// <param name="customServerInfo">Custom server information</param>
        /// <param name="host">Host</param>
        /// <param name="method">Method</param>
        /// <param name="ipv4Address">IPv4 address</param>
        /// <param name="port">Port</param>
        /// <param name="referer">Referer</param>
        /// <param name="useHTTPS">Use HTTPS</param>
        /// <param name="userAgent">User agent</param>
        /// <param name="version">Version</param>
        internal AnnouncerClient(EAnnouncerAPI api, SAMPServersAPIRequestDataContract customServerInfo, string host, string method, string ipv4Address, ushort port, string referer, bool useHTTPS, string userAgent, string version)
        {
            API = api;
            CustomServerInfo = ((api == EAnnouncerAPI.SAMPServersAPI) ? customServerInfo : null);
            this.host = host;
            this.method = method;
            this.ipv4Address = ipv4Address;
            Port = port;
            this.referer = referer;
            UseHTTPS = useHTTPS;
            this.userAgent = userAgent;
            this.version = version;
        }

        /// <summary>
        /// Announce (asynchronous)
        /// </summary>
        /// <returns>HTTP status code task</returns>
        public Task<HttpStatusCode> AnnounceAsync()
        {
            Task<HttpStatusCode> ret = new Task<HttpStatusCode>(() =>
            {
                HttpStatusCode r = HttpStatusCode.BadRequest;
                try
                {
                    if ((Port != 0) && (!(string.IsNullOrWhiteSpace(Host))))
                    {
                        StringBuilder uri_builder = null;
                        switch (API)
                        {
                            case EAnnouncerAPI.Legacy:
                                if (!(string.IsNullOrWhiteSpace(Version)))
                                {
                                    uri_builder = new StringBuilder();
                                    uri_builder.Append(UseHTTPS ? "https://" : "http://");
                                    uri_builder.Append(Host);
                                    uri_builder.Append("/");
                                    uri_builder.Append(Version);
                                    uri_builder.Append("/announce/");
                                    uri_builder.Append(Port.ToString());
                                }
                                break;
                            case EAnnouncerAPI.SAMPServersAPI:
                                if (!(string.IsNullOrWhiteSpace(IPv4Address)))
                                {
                                    uri_builder = new StringBuilder();
                                    uri_builder.Append(UseHTTPS ? "https://" : "http://");
                                    uri_builder.Append(Host);
                                    uri_builder.Append("/v2/server/");
                                    uri_builder.Append(IPv4Address);
                                    uri_builder.Append(":");
                                    uri_builder.Append(Port.ToString());
                                }
                                break;
                        }
                        if (uri_builder != null)
                        {
                            HttpWebRequest http_web_request = WebRequest.CreateHttp(uri_builder.ToString());
                            if (http_web_request != null)
                            {
                                http_web_request.Headers.Add(HttpRequestHeader.Accept, "*/*");
                                if (!(string.IsNullOrWhiteSpace(UserAgent)))
                                {
                                    http_web_request.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
                                }
                                if (!(string.IsNullOrWhiteSpace(Referer)))
                                {
                                    http_web_request.Headers.Add(HttpRequestHeader.Referer, Referer);
                                }
                                http_web_request.Headers.Add(HttpRequestHeader.Host, Host);
                                http_web_request.Method = Method;
                                if (CustomServerInfo != null)
                                {
                                    using (Stream request_stream = http_web_request.GetRequestStream())
                                    {
                                        serializer.WriteObject(request_stream, CustomServerInfo);
                                    }
                                }
                                using (HttpWebResponse response = http_web_request.GetResponse() as HttpWebResponse)
                                {
                                    r = response.StatusCode;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                return r;
            });
            ret.Start();
            return ret;
        }
    }
}
