using Pamaxie.Api.Controllers;
using Test.TestBase;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_UnitTesting
{
    /// <summary>
    /// Testing class for <see cref="AnalysisController"/>
    /// </summary>
    public class AnalysisControllerBaseTest : ApiBaseTest<AnalysisController>
    {
        public AnalysisControllerBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
    }
}