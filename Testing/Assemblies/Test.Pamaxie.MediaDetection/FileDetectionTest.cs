using System.Collections.Generic;
using System.IO;
using Pamaxie.MediaDetection;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.MediaDetection_Test
{
    /// <summary>
    /// Testing class for <see cref="FileDetection"/>
    /// </summary>
    public class FileDetectionTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.FileLinks"/>
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithFileType => MemberData.FileLinksWithFileType;

        public FileDetectionTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Testing for determine the file type from a stream
        /// </summary>
        /// <param name="url">Url for the file we want to determine the filetype</param>
        /// <param name="expectedFileType">Expected file type</param>
        [Theory]
        [MemberData(nameof(FileLinksWithFileType))]
        public void DetermineFileType(string url, FileType expectedFileType)
        {
            //Get the stream of the file from the url
            Stream stream = FileDetectionUtilities.UrlToStream(url);
            //Write the 10 first bytes of the stream in Hex
            TestOutputHelper.WriteLine("First 10 bytes of file:");
            TestOutputHelper.WriteLine(stream.ReadExactly(10).ToHexString());
            //Get the list of file specifications the FileType is linked to through the id
            //This is done to show the expected magic number of the file type.
            TestOutputHelper.WriteLine("Expected:");
            IEnumerable<FileSpecification> listOfTypeSpecs = expectedFileType.GetFileSpecifications();
            foreach (FileSpecification fileSpec in listOfTypeSpecs)
                TestOutputHelper.WriteLine(fileSpec.Signature.ToHexString());

            //Test the DetermineFileType method to see if it returns the expected FileType it should.
            FileType fileType = stream.DetermineFileType()?.Value;
            Assert.NotNull(fileType);
            Assert.Equal(expectedFileType.GetType(), fileType.GetType());
            TestOutputHelper.WriteLine("\nFile type: ." + fileType.Extension);
        }
    }
}