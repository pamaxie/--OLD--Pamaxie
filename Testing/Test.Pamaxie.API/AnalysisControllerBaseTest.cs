using Pamaxie.Api.Controllers;
using Test.Base;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_Test
{
    /// <summary>
    /// Testing class for <see cref="AnalysisController"/>
    /// </summary>
    public sealed class AnalysisControllerTestBaseTest : ApiTestBase<AnalysisController>
    {
        public AnalysisControllerTestBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
    }
}