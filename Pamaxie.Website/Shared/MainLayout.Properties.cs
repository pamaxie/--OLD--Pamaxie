using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Pamaxie.Website.Shared.MainLayout
{
    public partial class MainLayout
    {
        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
        [Inject] private ProtectedSessionStorage ProtectedSessionStorage { get; set; }
    }
}