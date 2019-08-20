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
        /// Error log path
        /// </summary>
        private string errorLogPath;

        /// <summary>
        /// Host
        /// </summary>
        private string host;

        /// <summary>
        /// IPv4 address
        /// </summary>
        private string ipv4Address;

        /// <summary>
        /// Log path
        /// </summary>
        private string logPath;

        /// <summary>
        /// MEthod
        /// </summary>
        private string method;

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
        /// Error log path
        /// </summary>
        private string ErrorLogPath
        {
            get
            {
                if (errorLogPath == null)
                {
                    errorLogPath = string.Empty;
                }
                return errorLogPath;
            }
        }

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
        /// Log path
        /// </summary>
        public string LogPath
        {
            get
            {
                if (logPath == null)
                {
                    logPath = string.Empty;
                }
                return logPath;
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
        /// <param name="errorLogPath">Error log path</param>
        /// <param name="host">Host</param>
        /// <param name="ipv4Address">IPv4 address</param>
        /// <param name="logPath">Log path</param>
        /// <param name="method">Method</param>
        /// <param name="port">Port</param>
        /// <param name="referer">Referer</param>
        /// <param name="useHTTPS">Use HTTPS</param>
        /// <param name="userAgent">User agent</param>
        /// <param name="version">Version</param>
        internal AnnouncerClient(EAnnouncerAPI api, string errorLogPath, SAMPServersAPIRequestDataContract customServerInfo, string host, string ipv4Address, string logPath, string method, ushort port, string referer, bool useHTTPS, string userAgent, string version)
        {
            API = api;
            this.errorLogPath = errorLogPath;
            CustomServerInfo = ((api == EAnnouncerAPI.SAMPServersAPI) ? customServerInfo : null);
            this.host = host;
            this.ipv4Address = ipv4Address;
            this.logPath = logPath;
            this.method = method;
            Port = port;
            this.referer = referer;
            UseHTTPS = useHTTPS;
            this.userAgent = userAgent;
            this.version = version;
        }

        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="isError">Is error</param>
        private void Log(object message, bool isError)
        {
            try
            {
                (isError ? Console.Error : Console.Out)?.WriteLine(message);
                string path = (isError ? ErrorLogPath : LogPath);
                if (!(string.IsNullOrWhiteSpace(path)))
                {
                    File.AppendAllText(path, (message == null) ? "null" : message.ToString() + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
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
                                    uri_builder.Append("/v2/server");
                                    if (CustomServerInfo == null)
                                    {
                                        uri_builder.Append("/");
                                        uri_builder.Append(IPv4Address);
                                        uri_builder.Append(":");
                                        uri_builder.Append(Port.ToString());
                                    }
                                }
                                break;
                        }
                        if (uri_builder != null)
                        {
                            HttpWebRequest http_web_request = WebRequest.CreateHttp(uri_builder.ToString());
                            if (http_web_request != null)
                            {
                                Log("Requesting at \"" + http_web_request.Address + "\" with API \"" + API + "\"...", false);
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
                                    http_web_request.ContentType = "application/json";
                                    using (MemoryStream request_memory_stream = new MemoryStream())
                                    {
                                        serializer.WriteObject(request_memory_stream, CustomServerInfo);
                                        request_memory_stream.Seek(0L, SeekOrigin.Begin);
                                        http_web_request.ContentLength = request_memory_stream.Length;
                                        using (Stream request_stream = http_web_request.GetRequestStream())
                                        {
                                            if (request_stream != null)
                                            {
                                                request_memory_stream.CopyTo(request_stream);
                                            }
                                        }
#if DEBUG
                                        request_memory_stream.Seek(0L, SeekOrigin.Begin);
                                        byte[] data = new byte[request_memory_stream.Length];
                                        if (request_memory_stream.Read(data) == data.Length)
                                        {
                                            Log(Encoding.UTF8.GetString(data), false);
                                        }
#endif
                                    }
                                }
                                using (HttpWebResponse response = http_web_request.GetResponse() as HttpWebResponse)
                                {
                                    r = response.StatusCode;
                                    Log("\"" + http_web_request.Address + "\" responded with \"" + r + "\"", false);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log(e, true);
                }
                return r;
            });
            ret.Start();
            return ret;
        }
    }
}
