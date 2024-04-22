﻿using System.ComponentModel.DataAnnotations;

namespace WebProjektRazor.Models.User
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string Password { get; set; }
    }
}
