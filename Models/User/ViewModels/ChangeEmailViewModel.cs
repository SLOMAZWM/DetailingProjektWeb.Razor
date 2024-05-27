﻿using System.ComponentModel.DataAnnotations;

namespace WebProjektRazor.Models.User.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "Nowy email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "Potwierdzenie email jest wymagane.")]
        [Compare("NewEmail", ErrorMessage = "Emaile nie są zgodne.")]
        public string ConfirmEmail { get; set; }
    }
}
