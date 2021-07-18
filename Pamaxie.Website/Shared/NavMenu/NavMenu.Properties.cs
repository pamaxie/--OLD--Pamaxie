using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Database.Extensions.Sql.Data;
#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class NavMenu
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        //Render Frag for rendering the child content of the Website
        [Parameter] public RenderFragment ChildContent { get; set; }

        private ProfileData? Profile { get; set; }
        private bool UserHasAccount { get; set; } = true;
        private bool ShowCreateAccount { get; set; }


    }
}