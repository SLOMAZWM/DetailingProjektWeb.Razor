﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebProjektRazor.Models.User
{
    public enum UserRole
    {
        Client,
        Employee
    }

    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imie musi mieć conajmniej 3 znaki i mniej niz 50 znaków.", MinimumLength = 3)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko musi mieć conajmniej 3 znaki i mniej niz 50 znaków.", MinimumLength = 3)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(256, ErrorMessage = "Hasło musi mieć od 8 do 256 znaków", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s:]).*$", ErrorMessage = "Hasło musi zawierać co najmniej jedną dużą literę, jedną małą literę, jedną cyfrę i jeden znak specjalny.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [RegularExpression(@"^\d{3}\s\d{3}\s\d{3}$", ErrorMessage = "Numer telefonu musi być w formacie 000 000 000.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public User() { }

    }

}
