using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;
using Pamaxie.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
#pragma warning disable 8618

namespace Pamaxie.Website.Pages.Dashboard
{
    public partial class DashBoard
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private List<Application> Applications { get; set; } = new();
        private Application? NewApplication { get; set; }
        private ProfileData? Profile { get; set; }
        private bool AcceptedTerms { get; set; }
        private bool AcceptedTos { get; set; }
        
        private MudTextField<string>? PwField1 { get; set; }
    }
}