using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes;

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
        public static readonly List<object[]> ListOfFileLinks = new()
        {
            new object[]
            {
                //png file
                "https://cdn.discordapp.com/attachments/439855996974661653/858971443609468948/81310947.png",
                "DFEB01001A60155CE530A5B7D754B08479B48D17FDAC2D851A2DF193CF368C66",
                new Png()
            },
            new object[]
            {
                //gif file
                "https://cdn.discordapp.com/emojis/781964894516805632.gif",
                "B40B6558E145F1DD8F4671E5F6DDB826EA56A78CFAD90E66E0393BFCEA807DC2",
                new Gif()
            },
            new object[]
            {
                //Zip file
                "https://cdn.discordapp.com/attachments/439855996974661653/885105541439967232/yv345c45x3f.zip",
                "1110A3866BEBEC592BFD93BEF911D6E88E9EB48AF3C3DED4AA4855FF080015AC",
                new Zip()
            },
            new object[]
            {
                //Empty zip file
                "https://cdn.discordapp.com/attachments/628844643831775234/885113691136213012/c2rwfw.zip",
                "8739C76E681F900923B900C9DF0EF75CF421D39CABB54650C4B9AD19B6A76D85",
                new Zip()
            },
            new object[]
            {
                //7z file
                "https://cdn.discordapp.com/attachments/439855996974661653/885105537589579776/yv345c45x3f.7z",
                "75B2557EF1A6691F6ACA48EC630074C91B160572243226A4318BA30D0264B303",
                new _7z()
            }
        };
    }
}