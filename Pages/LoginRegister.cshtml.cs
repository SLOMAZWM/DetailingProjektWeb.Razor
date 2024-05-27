using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProjektRazor.Models.User.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WebProjektRazor.Pages
{
    public class LoginRegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginRegisterModel(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterUser? RegisterUser { get; set; }

        [BindProperty]
        public LoginUserViewModel? LoginUser { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("UserType") != null)
            {
                var userType = HttpContext.Session.GetString("UserType");
                var redirectUrl = userType == "Client" ? "/ClientPage/ClientUserPanel" : "/EmployeePage/EmployeeUserPanel";
                return RedirectToPage(redirectUrl);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                UserName = RegisterUser.Email,
                Email = RegisterUser.Email,
                FirstName = RegisterUser.FirstName,
                LastName = RegisterUser.LastName,
                PhoneNumber = RegisterUser.PhoneNumber,
                Role = UserRole.Client
            };

            var result = await _userManager.CreateAsync(user, RegisterUser.Password);

            if (result.Succeeded)
            {
                var client = new Client
                {
                    UserId = user.Id
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserType", "Client");
                return RedirectToPage("ClientPage/ClientUserPanel");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(LoginUser.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, LoginUser.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserType", user.Role == UserRole.Client ? "Client" : "Employee");

                    string redirectPage = user.Role == UserRole.Client ? "ClientPage/ClientUserPanel" : "EmployeePage/EmployeeUserPanel";
                    return RedirectToPage(redirectPage);
                }
            }

            ModelState.AddModelError(string.Empty, "Nieprawid³owy email lub has³o.");
            return Page();
        }
    }
}
