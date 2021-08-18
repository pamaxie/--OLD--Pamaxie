using Microsoft.AspNetCore.Components;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Class containing method for mocking NavigationManager.
    /// </summary>
    internal static class MockNavigationManager
    {
        /// <summary>
        /// Mocks the NavigationManager so a url can be made for the EmailSender.
        /// </summary>
        /// <returns>Mocked NavigationManager</returns>
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