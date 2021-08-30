using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Extensions.Client
{
    /// <summary>
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public class UserDataServiceTest : Base.Test
    {
        public UserDataServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test()
        {
            string instance = "";
            string password = "";
            PamaxieDataContext context = new PamaxieDataContext(instance, password);
            DatabaseService service = new(context);
            IPamaxieUser user = new PamaxieUser();
            user.Create();
        }
    }
}