using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Sql.Data;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Dashboard
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private List<Application> Applications { get; set; } = new();
        private Application? NewApplication { get; set; } = null;
        private ProfileData? Profile { get; set; }
        private bool AcceptedTerms { get; set; }
        private bool AcceptedTos { get; set; }
        private MudTextField<string> PwField1 { get; set; }
    }
}