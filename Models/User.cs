namespace WebProjektRazor.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmployer { get; set; }

        public User(int userId = 0, string firstName = "", string lastName = "", string email = "", string password = "", string phoneNumber = "", bool isEmployer = false)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            IsEmployer = isEmployer;
        }

    }

}
