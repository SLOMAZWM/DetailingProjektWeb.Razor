using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User.ViewModel;
using Microsoft.AspNetCore.Http;
using WebProjektRazor.Models.User;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace WebProjektRazor.Pages.Shared
{
    public class ChangeDataUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChangeDataUserModel> _logger;

        public ChangeDataUserModel(ApplicationDbContext context, ILogger<ChangeDataUserModel> logger)
        {
            _context = context;
            _logger = logger;
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

            CurrentUser = await _context.Users.FindAsync(userId.Value);
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
                CurrentUser = await _context.Users.FindAsync(userId.Value);
            }
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            await LoadCurrentUserAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                _logger.LogWarning("User session not found, redirecting to login.");
                return RedirectToPage("/LoginRegister");
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                }
                return Page();
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null || !BCrypt.Net.BCrypt.Verify(ChangePasswordData.CurrentPassword, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Aktualne has³o jest nieprawid³owe.");
                _logger.LogWarning("Invalid current password provided.");
                return Page();
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(ChangePasswordData.NewPassword);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Password changed successfully for user {userId.Value}");
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

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            user.Email = ChangeEmailData.NewEmail;
            await _context.SaveChangesAsync();

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

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return RedirectToPage("/LoginRegister");
            }

            user.PhoneNumber = ChangePhoneNumberData.NewPhoneNumber;
            await _context.SaveChangesAsync();

            return RedirectToPage("/ChangeDataUser");
        }
    }
}
