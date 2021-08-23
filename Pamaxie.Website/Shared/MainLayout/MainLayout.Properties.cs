using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Pamaxie.Data;

#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="MainLayout"/>
    /// </summary>
    public partial class MainLayout
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
        
        private IPamaxieUser? Profile { get; set; }
    }
}