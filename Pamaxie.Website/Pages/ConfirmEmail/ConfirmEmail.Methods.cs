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
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Token));
            
            if (UserService.ConfirmEmail(code));
            {
                NavigationManager.NavigateTo(NavigationManager.BaseUri);
            }
            return Task.CompletedTask;
        }
    }
}