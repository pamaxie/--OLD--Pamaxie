using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;
using Pamaxie.Data;
using System.Collections.Generic;

namespace Pamaxie.Website.Pages.Dashboard
{
    public partial class DashBoard
    {
        [Inject] private IDialogService DialogService { get; set; }
        private List<Application> Applications { get; set; } = new();
        private Application? NewApplication { get; set; }
        private ProfileData? Profile { get; set; }
        private bool AcceptedTerms { get; set; }
        private bool AcceptedTos { get; set; }
        private MudTextField<string>? PwField1;
    }
}