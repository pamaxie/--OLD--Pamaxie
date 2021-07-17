using System.Diagnostics;
using Xunit;
using ImageProcessing = Pamaxie.ImageProcessing.ImageProcessing;

namespace Test.Pamaxie.ImageProcessing_UnitTesting
{
    /// <summary>
    /// Testing class for ImageProcessing
    ///BUG: we cannot test things with data on any external system that doesn't run dependent on the test. The test has to run no matter on external systems or servers.
    /// A way to fix this would be to implement a test with a static stream of data and the hash it generates, this would be much more preferred.
    /// The Download class has to be interfaced and a seperate interface implementation for testing would be nessecary.
    /// </summary>
    public class ImageProcessingTest
    {
        /// <summary>
        /// Testing for succeeding in downloading a url file
        /// </summary>
        /// <param name="url">Url of the file you want to download</param>
        [Theory]
        [InlineData("https://cdn.discordapp.com/attachments/439855996974661653/858971443609468948/81310947.png")]
        [InlineData("https://cdn.discordapp.com/emojis/781964894516805632.gif")]
        public void DownloadFile_Succeed(string url)
        {

            //Once FileDetection.DetermineFileType() have been fixed, then make this test method run through the
            //FileDetection.DetermineFileType() again to determine if the file have been saved to the original filetype
            //and can be loaded.
            var file = ImageProcessing.DownloadFile(url);
            Assert.NotNull(file);
        }
        
        /// <summary>
        /// Testing for failure in downloading a url file
        /// BUG: Currently crashes, since no file can be found.
        /// </summary>
        /// <param name="url">Url of the file you want to download</param>
        //[Theory]
        //[InlineData("https://cdn.discordapp.com/emojis/78196489451680563")]
        //public void DownloadFile_Failure(string url)
        //{
        //    //This will be done by trying to send a url that is not a file.
        //    var file = ImageProcessing.DownloadFile(url);
        //    Assert.Null(file);
        //}

        /// <summary>
        /// Testing for succeeding in getting the file hash from a url file
        /// </summary>
        /// <param name="url">Url of the file you want to get the file hash from</param>
        /// <param name="expected">The expected file hash from the url file</param>
        [Theory]
        [InlineData("https://cdn.discordapp.com/attachments/439855996974661653/858971443609468948/81310947.png",
            "dfeb01001a60155ce530a5b7d754b08479b48d17fdac2d851a2df193cf368c66")]
        [InlineData("https://cdn.discordapp.com/emojis/781964894516805632.gif",
            "b40b6558e145f1dd8f4671e5f6ddb826ea56a78cfad90e66e0393bfcea807dc2")]
        public void GetFileHash_Success(string url, string expected)
        {
            var fileHash = ImageProcessing.GetFileHash(url).Result;
            Assert.Equal(expected, fileHash);
        }
    }
}