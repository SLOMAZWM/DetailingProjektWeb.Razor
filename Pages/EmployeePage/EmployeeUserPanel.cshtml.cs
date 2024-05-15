using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProjektRazor.Pages.EmployeePage
{
    public class EmployeeUserPanelModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if(userType != "Employee")
            {
                return RedirectToPage("/LoginRegister");
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/LoginRegister");
        }
    }
}
