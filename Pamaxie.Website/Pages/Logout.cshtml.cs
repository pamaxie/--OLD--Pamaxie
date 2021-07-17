using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Pamaxie.Website.Pages
{
    public class LogoutModel : PageModel
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public async Task<IActionResult> OnGetAsync()
        {
            // Clear the existing external cookie
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
                // ignored
            }

            return LocalRedirect("/");
        }
    }
}
