using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Pamaxie.Database.Extensions.Sql.Data;
#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class MainLayout
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
        private ProfileData? Profile { get; set; }
    }
}