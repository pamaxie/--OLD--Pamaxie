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
            Stream stream = FileDetectionUtilities.UrlToStream(url);
            TestOutputHelper.WriteLine("First 10 bytes of file:");
            TestOutputHelper.WriteLine(stream.ReadExactly(10).ToHexString());
            TestOutputHelper.WriteLine("Expected:");
            IEnumerable<FileSpecification> listOfTypeSpecs = expectedFileType.GetFileSpecifications();
            foreach (FileSpecification fileSpec in (listOfTypeSpecs))
            {
                TestOutputHelper.WriteLine(fileSpec.Signature.ToHexString());
            }
            
            KeyValuePair<FileSpecification, FileType>? fileInformation = stream.DetermineFileType();
            Assert.NotNull(fileInformation);
            Assert.Equal(expectedFileType.GetType(), fileInformation.Value.Value.GetType());
            TestOutputHelper.WriteLine("\nfile type: ." + fileInformation.Value.Value.Extension);
        }
    }
}