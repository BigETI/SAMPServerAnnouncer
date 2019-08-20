using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// Announcer
    /// </summary>
    public class Announcer
    {
        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ConfigDataContract), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });

        /// <summary>
        /// Announcer clients
        /// </summary>
        private AnnouncerClient[] announcerClients;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="announcerClients">Announcer clients</param>
        internal Announcer(AnnouncerClient[] announcerClients)
        {
            this.announcerClients = announcerClients;
        }

        /// <summary>
        /// Announce (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task AnnounceAsync()
        {
            Task ret = new Task((announcer) =>
            {
                if (announcerClients != null)
                {
                    foreach (AnnouncerClient announcer_client in announcerClients)
                    {
                        announcer_client?.AnnounceAsync().GetAwaiter().GetResult();
                    }
                }
            }, this);
            ret.Start();
            return ret;
        }

        /// <summary>
        /// Create announcer
        /// </summary>
        /// <param name="configStream">Configuration stream</param>
        /// <param name="defaultPort">Default port</param>
        /// <param name="disposeConfigStreamWhenDone">Dispose config stream when done</param>
        /// <returns>Announcer if successful, otherwise "null"</returns>
        public static Announcer Create(Stream configStream, ushort defaultPort, bool disposeConfigStreamWhenDone)
        {
            Announcer ret = null;
            if (configStream != null)
            {
                if (configStream.CanRead)
                {
                    ConfigDataContract config = serializer.ReadObject(configStream) as ConfigDataContract;
                    if (config != null)
                    {
                        List<AnnouncerClient> announcer_clients = new List<AnnouncerClient>();
                        string ipv4_address = null;
                        foreach (AnnouncereDataContract announcer in config.Announcers)
                        {
                            if (announcer != null)
                            {
                                if (announcer.Port == 0)
                                {
                                    announcer.Port = defaultPort;
                                }
                                if (announcer.IsValid)
                                {
                                    try
                                    {
                                        if ((announcer.API == EAnnouncerAPI.SAMPServersAPI) && (ipv4_address == null))
                                        {
                                            ipv4_address = config.IPv4Address;
                                            if (ipv4_address.Length <= 0)
                                            {
                                                foreach (string ipv4_service_uri in config.IPv4ServiceURIs)
                                                {
                                                    if (!(string.IsNullOrWhiteSpace(ipv4_service_uri)))
                                                    {
                                                        Uri uri = new Uri(ipv4_service_uri.Trim());
                                                        HttpWebRequest ipv4_address_http_web_request = WebRequest.CreateHttp(uri);
                                                        if (ipv4_address_http_web_request != null)
                                                        {
                                                            ipv4_address_http_web_request.Headers.Add(HttpRequestHeader.Accept, "*/*");
                                                            ipv4_address_http_web_request.Headers.Add(HttpRequestHeader.UserAgent, config.UserAgent);
                                                            ipv4_address_http_web_request.Headers.Add(HttpRequestHeader.Host, uri.Host);
                                                            using (HttpWebResponse response = ipv4_address_http_web_request.GetResponse() as HttpWebResponse)
                                                            {

                                                                if (response.StatusCode == HttpStatusCode.OK)
                                                                {
                                                                    using (Stream response_stream = response.GetResponseStream())
                                                                    {
                                                                        using (StreamReader response_reader = new StreamReader(response_stream))
                                                                        {
                                                                            ipv4_address = response_reader.ReadToEnd().Trim();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.Error.WriteLine(e);
                                    }
                                    if ((announcer.API != EAnnouncerAPI.SAMPServersAPI) || (ipv4_address != null))
                                    {
                                        announcer_clients.Add(new AnnouncerClient(announcer.API, config.ErrorLogPath, announcer.UseCustomServerInfo ? config.CustomServerInfo : null, announcer.Host, (announcer.API == EAnnouncerAPI.SAMPServersAPI) ? ipv4_address : string.Empty, config.LogPath, announcer.Method, announcer.Port, announcer.Referer, announcer.UseHTTPS, announcer.UserAgent, announcer.Version));
                                    }
                                }
                            }
                        }
                        ret = new Announcer(announcer_clients.ToArray());
                        announcer_clients.Clear();
                    }
                }
                if (disposeConfigStreamWhenDone)
                {
                    configStream.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// Create announcer
        /// </summary>
        /// <param name="configStream">Configuration stream</param>
        /// <param name="defaultPort">Default port</param>
        /// <returns>Announcer if successful, otherwise "null"</returns>
        public static Announcer Create(Stream configStream, ushort defaultPort)
        {
            return Create(configStream, defaultPort, false);
        }

        /// <summary>
        /// Create announcer
        /// </summary>
        /// <param name="configPath">Configuration path</param>
        /// <param name="defaultPort">Default port</param>
        /// <returns>Announcer if successful, otherwise "null"</returns>
        public static Announcer Create(string configPath, ushort defaultPort)
        {
            Announcer ret = null;
            if (configPath != null)
            {
                if (File.Exists(configPath))
                {
                    using (FileStream config_file_stream = File.OpenRead(configPath))
                    {
                        if (config_file_stream != null)
                        {
                            ret = Create(config_file_stream, defaultPort, false);
                        }
                    }
                }
            }
            return ret;
        }
    }
}
