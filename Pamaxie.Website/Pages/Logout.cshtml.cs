using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pamaxie.Website.Pages
{
    public class LogoutModel : PageModel
    {
        // ReSharper disable once UnusedMember.Global
        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
                // ignored
            }

            return LocalRedirect(returnUrl);
        }
    }
}
