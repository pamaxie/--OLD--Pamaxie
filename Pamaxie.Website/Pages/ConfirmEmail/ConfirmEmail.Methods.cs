using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class ConfirmEmail
    {
        protected override Task OnInitializedAsync()
        {
            if (UserService.ConfirmEmail(Token));
            {
                NavigationManager.NavigateTo(NavigationManager.BaseUri);
            }
            return Task.CompletedTask;
        }
    }
}