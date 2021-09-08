using Microsoft.AspNetCore.Components;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking <see cref="NavigationManager"/>.
    /// </summary>
    public static class MockNavigationManager
    {
        /// <summary>
        /// Mocks the <see cref="NavigationManager"/> so a url can be made for the EmailSender.
        /// </summary>
        /// <returns>Mocked NavigationManager</returns>
        public static NavigationManager Mock()
        {
            return new MockedNavigationManager();
        }

        private sealed class MockedNavigationManager : NavigationManager
        {
            public MockedNavigationManager() =>
                Initialize("http://localhost:2112/", "http://localhost:2112/test");

            protected override void NavigateToCore(string uri, bool forceLoad)
            {
            }
        }
    }
}