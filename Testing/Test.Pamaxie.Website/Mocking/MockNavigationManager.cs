using Microsoft.AspNetCore.Components;

namespace Test.Pamaxie.Website
{
    internal static class MockNavigationManager
    {
        internal static NavigationManager Mock()
        {
            return new MockedNavigationManager();
        }
        
        private sealed class MockedNavigationManager : NavigationManager
        {
            public MockedNavigationManager() => 
                Initialize("http://localhost:2112/", "http://localhost:2112/test");

            protected override void NavigateToCore(string uri, bool forceLoad) { }
        }
    }
}