using WebProjektRazor.Models;
using WebProjektRazor.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProjektRazor.Pages
{
    public class LoginRegisterModel : PageModel
    {
        [BindProperty]
        public User RegisterUser { get; set; } 

        [BindProperty]
        public User LoginUser { get; set; }  

        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPostRegisterAsync()
        {
            try
            {
                await UserDatabase.AddUserToDatabase(RegisterUser);  
                return RedirectToPage("/UserPanel");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst¹pi³ b³¹d podczas rejestracji: " + ex.Message);
                return Page();
            }
        }

        //public async Task<IActionResult> OnPostLoginAsync()
        //{

        //    return RedirectToPage("/UserPanel");
        //}
    }
}
