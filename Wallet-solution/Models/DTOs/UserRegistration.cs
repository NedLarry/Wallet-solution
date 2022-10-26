using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Wallet_solution.Models.DTOs
{
    public class UserRegistration
    {
        public UserRegistration(string Firstname, string lastname, string Email, string Password)
        {

            this.FirstName = Firstname;
            this.LastName = lastname;
            this.Email = Email;
            this.Password = Password;

        }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(10, MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
