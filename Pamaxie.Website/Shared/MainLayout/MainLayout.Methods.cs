using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class MainLayout
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                ProtectedBrowserStorageResult<bool> result = await ProtectedLocalStorage.GetAsync<bool>("useCookies");
                bool currentValue = result.Success && result.Value;
                _showCookieDialog = !currentValue;
                StateHasChanged();
            }
            catch
            {
                await ProtectedLocalStorage.DeleteAsync("useCookies");
                _showCookieDialog = false;
            }
        }

        private async Task Save()
        {
            await ProtectedLocalStorage.SetAsync("useCookies", true);
        }
    }
}