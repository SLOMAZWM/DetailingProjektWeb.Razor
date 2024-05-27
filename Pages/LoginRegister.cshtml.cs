using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace WebProjektRazor.Pages
{
    public class LoginRegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginRegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterUser? RegisterUser { get; set; }

        [BindProperty]
        public LoginUser? LoginUser { get; set; }

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
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(RegisterUser, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(RegisterUser, validationContext, validationResults, true);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage);
                }
                return Page();
            }

            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(RegisterUser.Password);
                var user = new User
                {
                    FirstName = RegisterUser.FirstName,
                    LastName = RegisterUser.LastName,
                    Email = RegisterUser.Email,
                    Password = hashedPassword,
                    PhoneNumber = RegisterUser.PhoneNumber,
                    Role = UserRole.Client
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var client = new Client
                {
                    UserId = user.Id
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserType", "Client");
                return RedirectToPage("ClientPage/ClientUserPanel");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst¹pi³ b³¹d podczas rejestracji: " + ex.Message);
                return Page();
            }
        }


        public async Task<IActionResult> OnPostLoginAsync()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(LoginUser, serviceProvider: null, items: null);

            if (!Validator.TryValidateObject(LoginUser, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage);
                }
                return Page();
            }

            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == LoginUser.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(LoginUser.Password, user.Password))
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserType", user.Role == UserRole.Client ? "Client" : "Employee");

                    string redirectPage = user.Role == UserRole.Client ? "ClientPage/ClientUserPanel" : "EmployeePage/EmployeeUserPanel";
                    return RedirectToPage(redirectPage);
                }
                ModelState.AddModelError("", "Nieprawid³owy email lub has³o.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst¹pi³ b³¹d podczas logowania: " + ex.Message);
            }
            return Page();
        }
    }
}
