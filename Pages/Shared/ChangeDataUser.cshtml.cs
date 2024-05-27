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

            if (string.IsNullOrEmpty(ChangePasswordData.CurrentPassword) ||
                string.IsNullOrEmpty(ChangePasswordData.NewPassword) ||
                string.IsNullOrEmpty(ChangePasswordData.ConfirmPassword))
            {
                TempData["ErrorMessage"] = "Wszystkie pola s¹ wymagane.";
                return RedirectToPage();
            }

            if (ChangePasswordData.NewPassword != ChangePasswordData.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Nowe has³o i potwierdzenie has³a nie s¹ zgodne.";
                return RedirectToPage();
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(CurrentUser, ChangePasswordData.CurrentPassword);
            if (!passwordCheck)
            {
                TempData["ErrorMessage"] = "Aktualne has³o jest nieprawid³owe.";
                return RedirectToPage();
            }

            var result = await _userManager.ChangePasswordAsync(CurrentUser, ChangePasswordData.CurrentPassword, ChangePasswordData.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            TempData["SuccessMessage"] = "Has³o zosta³o pomyœlnie zmienione.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (string.IsNullOrEmpty(ChangeEmailData.NewEmail) ||
                string.IsNullOrEmpty(ChangeEmailData.ConfirmEmail))
            {
                TempData["ErrorMessage"] = "Wszystkie pola s¹ wymagane.";
                return RedirectToPage();
            }

            if (ChangeEmailData.NewEmail != ChangeEmailData.ConfirmEmail)
            {
                TempData["ErrorMessage"] = "Nowy email i potwierdzenie emaila nie s¹ zgodne.";
                return RedirectToPage();
            }

            var result = await _userManager.SetEmailAsync(CurrentUser, ChangeEmailData.NewEmail);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            TempData["SuccessMessage"] = "Email zosta³ pomyœlnie zmieniony.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePhoneNumberAsync()
        {
            await LoadCurrentUserAsync();

            if (CurrentUser == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            if (string.IsNullOrEmpty(ChangePhoneNumberData.NewPhoneNumber) ||
                string.IsNullOrEmpty(ChangePhoneNumberData.ConfirmPhoneNumber))
            {
                TempData["ErrorMessage"] = "Wszystkie pola s¹ wymagane.";
                return RedirectToPage();
            }

            if (ChangePhoneNumberData.NewPhoneNumber != ChangePhoneNumberData.ConfirmPhoneNumber)
            {
                TempData["ErrorMessage"] = "Nowy numer telefonu i potwierdzenie numeru telefonu nie s¹ zgodne.";
                return RedirectToPage();
            }

            var result = await _userManager.SetPhoneNumberAsync(CurrentUser, ChangePhoneNumberData.NewPhoneNumber);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(CurrentUser);

            TempData["SuccessMessage"] = "Numer telefonu zosta³ pomyœlnie zmieniony.";
            return RedirectToPage();
        }
    }
}
