using System.Collections.Generic;
using System.IO;
using Pamaxie.ImageProcessing;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.ImageProcessing_Test
{
    /// <summary>
    /// Testing class for <see cref="ImageProcessing"/>
    /// </summary>
    public class ImageProcessingTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.FileLinks"/>
        /// </summary>
        public static IEnumerable<object[]> FileLinks => MemberData.FileLinks;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.FileLinksWithHash"/>
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithHash => MemberData.FileLinksWithHash;
        
        public ImageProcessingTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Testing for downloading a file from a url
        /// </summary>
        /// <param name="url">A url to the file that will be downloaded</param>
        [Theory]
        [MemberData(nameof(FileLinks))]
        public void DownloadFile(string url)
        {
            FileInfo file = ImageProcessing.DownloadFile(url);
            Assert.NotNull(file);
            TestOutputHelper.WriteLine(file.FullName);
        }

        /// <summary>
        /// Testing for getting a file's hash
        /// </summary>
        /// <param name="url">A url to the file that will be checked for it's hash</param>
        /// <param name="expectedHash">The expected file hash</param>
        [Theory]
        [MemberData(nameof(FileLinksWithHash))]
        public void GetFileHash(string url, string expectedHash)
        {
            string fileHash = ImageProcessing.GetFileHash(url).Result.ToUpper();
            Assert.Equal(expectedHash, fileHash);
        }
    }
}