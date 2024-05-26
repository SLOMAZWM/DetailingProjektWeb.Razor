using System.ComponentModel.DataAnnotations;

namespace WebProjektRazor.Models.User.ViewModel
{
    public class ChangeDataUserViewModel
    {
        [Required(ErrorMessage = "Aktualne hasło jest wymagane.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Nowe hasło jest wymagane.")]
        [StringLength(256, ErrorMessage = "Hasło musi mieć od 8 do 256 znaków", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s:]).*$", ErrorMessage = "Hasło musi zawierać co najmniej jedną dużą literę, jedną małą literę, jedną cyfrę i jeden znak specjalny.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Aktualny email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string CurrentEmail { get; set; }

        [Required(ErrorMessage = "Nowy email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "Potwierdzenie email jest wymagane.")]
        [Compare("NewEmail", ErrorMessage = "Emaile nie są zgodne.")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Aktualny numer telefonu jest wymagany.")]
        [RegularExpression(@"^\+\d{1,3}\s\d{1,3}\s\d{3}\s\d{3}\s\d{3}$", ErrorMessage = "Numer telefonu musi być w formacie +00 000 000 000.")]
        public string CurrentPhoneNumber { get; set; }

        [Required(ErrorMessage = "Nowy numer telefonu jest wymagany.")]
        [RegularExpression(@"^\+\d{1,3}\s\d{1,3}\s\d{3}\s\d{3}\s\d{3}$", ErrorMessage = "Numer telefonu musi być w formacie +00 000 000 000.")]
        public string NewPhoneNumber { get; set; }

        [Required(ErrorMessage = "Potwierdzenie numeru telefonu jest wymagane.")]
        [Compare("NewPhoneNumber", ErrorMessage = "Numery telefonów nie są zgodne.")]
        public string ConfirmPhoneNumber { get; set; }
    }
}
