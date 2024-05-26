using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User.ViewModel;
using Microsoft.AspNetCore.Http;
using WebProjektRazor.Models.User;

namespace WebProjektRazor.Pages.Shared
{
    public class ChangeDataUserModel : PageModel
    {
        [BindProperty]
        public ChangeDataUserViewModel ChangeData { get; set; }

        public User CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            CurrentUser = await UserDatabase.GetUserById(userId.Value);
            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            // Inicjalizacja w³aœciwoœci ChangeData z aktualnymi danymi u¿ytkownika
            ChangeData = new ChangeDataUserViewModel
            {
                CurrentEmail = CurrentUser.Email,
                CurrentPhoneNumber = CurrentUser.PhoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            var result = await UserDatabase.UpdateUserPassword(userId.Value.ToString(), ChangeData.CurrentPassword, ChangeData.NewPassword);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Aktualne has³o jest nieprawid³owe.");
                return Page();
            }

            return RedirectToPage("/LoginRegister");
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            var result = await UserDatabase.UpdateUserEmail(userId.Value.ToString(), ChangeData.NewEmail);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zmieniæ emaila.");
                return Page();
            }

            return RedirectToPage("/LoginRegister");
        }

        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            var result = await UserDatabase.UpdateUserPhoneNumber(userId.Value.ToString(), ChangeData.NewPhoneNumber);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zmieniæ numeru telefonu.");
                return Page();
            }

            return RedirectToPage("/LoginRegister");
        }
    }
}
