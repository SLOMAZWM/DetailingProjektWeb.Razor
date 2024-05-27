using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User.ViewModels;
using Microsoft.AspNetCore.Http;
using WebProjektRazor.Models.User;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace WebProjektRazor.Pages.Shared
{
    public class ChangeDataUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChangeDataUserModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ChangeDataUserModel(
            ApplicationDbContext context,
            ILogger<ChangeDataUserModel> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

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

            CurrentUser = await _userManager.FindByIdAsync(userId.Value.ToString());
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
                CurrentUser = await _userManager.FindByIdAsync(userId.Value.ToString());
            }
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userManager.ChangePasswordAsync(CurrentUser, ChangePasswordData.CurrentPassword, ChangePasswordData.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            _logger.LogInformation("User changed their password successfully.");
            return RedirectToPage("/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userManager.SetEmailAsync(CurrentUser, ChangeEmailData.NewEmail);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            _logger.LogInformation("User changed their email successfully.");
            return RedirectToPage("/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userManager.SetPhoneNumberAsync(CurrentUser, ChangePhoneNumberData.NewPhoneNumber);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            _logger.LogInformation("User changed their phone number successfully.");
            return RedirectToPage("/ChangeDataUser");
        }
    }
}
