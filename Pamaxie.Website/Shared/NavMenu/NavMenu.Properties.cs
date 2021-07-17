using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;

namespace Pamaxie.Website.Shared.NavMenu
{
    public partial class NavMenu
    {
        [Inject]
        private IDialogService DialogService { get; set; }

        //Render Frag for rendering the child content of the Website
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ProfileData Profile { get; set; }

        private bool UserHasAccount { get; set; } = true;
        private bool ShowCreateAccount { get; set; }


    }
}