using System.Collections.Generic;

//Get-FileHash [-path] -Algorithm SHA256 | Format-List
namespace Test.Base
{
    /// <summary>
    /// Contains file link Data
    /// </summary>
    public static class TestFileLinkData
    {
        /// <summary>
        /// List of file links
        /// </summary>
        public static readonly List<string[]> ListOfFileLinks = new()
        {
            new[]
            {
                "https://cdn.discordapp.com/attachments/439855996974661653/858971443609468948/81310947.png",
                "DFEB01001A60155CE530A5B7D754B08479B48D17FDAC2D851A2DF193CF368C66"
            },
            new[]
            {
                "https://cdn.discordapp.com/emojis/781964894516805632.gif",
                "B40B6558E145F1DD8F4671E5F6DDB826EA56A78CFAD90E66E0393BFCEA807DC2"
            },
            new[]
            {
                "https://cdn.discordapp.com/attachments/439855996974661653/885086422216876032/yv345c45x3f.txt",
                "532EAABD9574880DBF76B9B8CC00832C20A6EC113D682299550D7A6E0F345E25"
            }
        };
    }
}