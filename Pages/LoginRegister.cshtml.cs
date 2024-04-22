using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebProjektRazor.Database;
using WebProjektRazor.Models.User;

namespace WebProjektRazor.Pages
{
    public class LoginRegisterModel : PageModel
    {
        [BindProperty]
        public RegisterUser? RegisterUser { get; set; }

        [BindProperty]
        public LoginUser? LoginUser { get; set; }

        public void OnGet()
        {
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
                var client = await UserDatabase.AddUserToDatabase(RegisterUser);
                if (client != null)
                {
                    return RedirectToPage("/UserPanel");
                }
                else
                {
                    ModelState.AddModelError("", "Nie uda³o siê zarejestrowaæ u¿ytkownika.");
                    return Page();
                }
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
                var client = await UserDatabase.TryLoginUser(LoginUser.Email, LoginUser.Password);
                if (client != null)
                {
                    return RedirectToPage("/UserPanel");
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
