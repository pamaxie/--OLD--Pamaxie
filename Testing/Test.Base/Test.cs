using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Test.Base
{
    public class Test
    {
        protected ITestOutputHelper TestOutputHelper { get; }

        protected static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

        // ReSharper disable once MemberCanBeProtected.Global //Must be Public!
        public Test(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }
}