using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebProjektRazor.Models.User;

namespace WebProjektRazor.Pages.EmployeePage
{
    public class EmployeeUserPanelModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public EmployeeUserPanelModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee")
            {
                return RedirectToPage("/LoginRegister");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToPage("/LoginRegister");
        }
    }
}
