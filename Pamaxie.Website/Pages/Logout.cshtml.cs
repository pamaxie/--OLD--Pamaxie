using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pamaxie.Website.Pages
{
    /// <summary>
    /// Class for the Logout ViewModel page
    /// </summary>
    public class LogoutModel : PageModel
    {
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Log out the user
        /// </summary>
        /// <param name="returnUrl">Page to return to</param>
        /// <returns><see cref="IActionResult"/></returns>
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
