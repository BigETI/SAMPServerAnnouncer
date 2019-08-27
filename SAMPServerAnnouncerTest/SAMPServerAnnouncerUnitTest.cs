using SAMPServerAnnouncer;
using System.IO;
using Xunit;

/// <summary>
/// San ANdreas Multiplayer announcer test namespace
/// </summary>
namespace SAMPServerAnnouncerTest
{
    /// <summary>
    /// San Andreas Multiplayer announcer unit test class
    /// </summary>
    public class SAMPServerAnnouncerUnitTest
    {
        /// <summary>
        /// Announce fake server
        /// </summary>
        [Fact]
        public void AnnounceFakeServer()
        {
            if (File.Exists("announce.log"))
            {
                File.Delete("announce.log");
            }
            if (File.Exists("announce-error.log"))
            {
                File.Delete("announce-error.log");
            }
            Announcer announcer = Announcer.Create("announce.json", 7777);
            Assert.NotNull(announcer);
            announcer.AnnounceAsync().GetAwaiter().GetResult();
            Assert.True(File.Exists("announce.log"), "No announce logs have been created. Possibly the announcers array is empty.");
            Assert.True(!(File.Exists("announce-error.log")), "There have been errors during requests. Check \"announce-error.log\" for more information.");
        }

        /// <summary>
        /// Announce fake server with custom server information
        /// </summary>
        [Fact]
        public void AnnounceFakeServerWithCustomServerInfo()
        {
            if (File.Exists("announce-custom.log"))
            {
                File.Delete("announce-custom.log");
            }
            if (File.Exists("announce-error-custom.log"))
            {
                File.Delete("announce-error-custom.log");
            }
            Announcer announcer = Announcer.Create("announce-custom.json", 7777);
            Assert.NotNull(announcer);
            announcer.AnnounceAsync().GetAwaiter().GetResult();
            Assert.True(File.Exists("announce-custom.log"), "No announce logs have been created. Possibly the announcers array is empty.");
            Assert.True(!(File.Exists("announce-error-custom.log")), "There have been errors during requests. Check \"announce-error-custom.log\" for more information.");
        }
    }
}
