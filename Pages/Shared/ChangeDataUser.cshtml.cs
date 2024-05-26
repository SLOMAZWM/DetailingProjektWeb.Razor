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
        public ChangePasswordViewModel ChangePasswordData { get; set; }

        [BindProperty]
        public ChangeEmailViewModel ChangeEmailData { get; set; }

        [BindProperty]
        public ChangePhoneNumberViewModel ChangePhoneNumberData { get; set; }

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

            ChangeEmailData = new ChangeEmailViewModel
            {
                CurrentEmail = CurrentUser.Email
            };

            ChangePhoneNumberData = new ChangePhoneNumberViewModel
            {
                CurrentPhoneNumber = CurrentUser.PhoneNumber
            };

            return Page();
        }

        private async Task LoadCurrentUserAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                CurrentUser = await UserDatabase.GetUserById(userId.Value);
            }
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            await LoadCurrentUserAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await UserDatabase.UpdateUserPassword(userId.Value.ToString(), ChangePasswordData.CurrentPassword, ChangePasswordData.NewPassword);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Aktualne has³o jest nieprawid³owe.");
                return Page();
            }

            return RedirectToPage("/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            await LoadCurrentUserAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await UserDatabase.UpdateUserEmail(userId.Value.ToString(), ChangeEmailData.NewEmail);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zmieniæ emaila.");
                return Page();
            }

            return RedirectToPage("/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
        {
            await LoadCurrentUserAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await UserDatabase.UpdateUserPhoneNumber(userId.Value.ToString(), ChangePhoneNumberData.NewPhoneNumber);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zmieniæ numeru telefonu.");
                return Page();
            }

            return RedirectToPage("/ChangeDataUser");
        }
    }
}
