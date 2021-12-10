using Pamaxie.Api.Controllers;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_Test
{
    /// <summary>
    /// Testing class for <see cref="AnalysisController"/>
    /// </summary>
    public sealed class AnalysisControllerTest : ApiTestBase<AnalysisController>
    {
        public AnalysisControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
    }
}