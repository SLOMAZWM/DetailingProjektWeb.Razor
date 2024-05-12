using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProjektRazor.Pages.ClientPage
{
    public class UserPanelModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Client")
            {
                return RedirectToPage("/LoginRegister");
            }

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/LoginRegister");
        }
    }
}
