using System.Threading.Tasks;

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global¨'
    /// <summary>
    /// Class for the Email Confirmation page
    /// </summary>
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