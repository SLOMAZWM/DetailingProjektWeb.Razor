using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User.ViewModels;
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User not found, redirecting to LoginRegister.");
                return RedirectToPage("/LoginRegister");
            }

            CurrentUser = user;

            ChangeEmailData = new ChangeEmailViewModel
            {
                CurrentEmail = user.Email
            };

            ChangePhoneNumberData = new ChangePhoneNumberViewModel
            {
                CurrentPhoneNumber = user.PhoneNumber
            };

            _logger.LogInformation("ChangeDataUser page loaded successfully.");
            return Page();
        }

        private async Task LoadCurrentUserAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(CurrentUser, ChangePasswordData.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Aktualne has³o jest nieprawid³owe.");
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

            TempData["SuccessMessage"] = "Has³o zosta³o pomyœlnie zmienione.";
            return RedirectToPage("/Shared/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
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

            TempData["SuccessMessage"] = "Email zosta³ pomyœlnie zmieniony.";
            return RedirectToPage("/Shared/ChangeDataUser");
        }

        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
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

            TempData["SuccessMessage"] = "Numer telefonu zosta³ pomyœlnie zmieniony.";
            return RedirectToPage("/Shared/ChangeDataUser");
        }
    }
}
