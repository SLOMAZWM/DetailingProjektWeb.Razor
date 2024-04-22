using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebProjektRazor.Models;
using WebProjektRazor.Database;

namespace WebProjektRazor.Pages
{
    public class LoginRegisterModel : PageModel
    {
        [BindProperty]
        public User? RegisterUser { get; set; }

        [BindProperty]
        public User? LoginUser { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(RegisterUser, serviceProvider: null, items: null);

            // R�czne wywo�anie walidacji
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
                    ModelState.AddModelError("", "Nie uda�o si� zarejestrowa� u�ytkownika.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst�pi� b��d podczas rejestracji: " + ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Nieprawid�owy email lub has�o.");
                return Page();
            }

            try
            {
                var client = await UserDatabase.TryLoginUser(LoginUser.Email, LoginUser.Password);
                if (client != null)
                {
                    return RedirectToPage("/UserPanel");
                }
                else
                {
                    ModelState.AddModelError("", "Nieprawid�owy email lub has�o.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst�pi� b��d podczas logowania: " + ex.Message);
                return Page();
            }
        }
    }
}
