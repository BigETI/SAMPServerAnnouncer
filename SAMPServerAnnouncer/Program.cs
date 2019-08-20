/// <summary>
/// San Andreas Multiplayer server announcer namespace
/// </summary>
namespace SAMPServerAnnouncer
{
    /// <summary>
    /// Program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        internal static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                ushort default_port;
                if (ushort.TryParse(args[1], out default_port))
                {
                    Announcer.Create("announce.json", default_port)?.AnnounceAsync().GetAwaiter().GetResult();
                }
            }
        }
    }
}
