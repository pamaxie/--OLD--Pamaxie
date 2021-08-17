using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    public class Test
    {
        internal ITestOutputHelper TestOutputHelper { get; }

        internal static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

        // ReSharper disable once MemberCanBeProtected.Global //Must be Public!
        public Test(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

    }
}