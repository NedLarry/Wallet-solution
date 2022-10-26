using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Wallet_solution.Models.DTOs
{
    public class UserLogin
    {

        public UserLogin(string Email, string Password)
        {

            this.Email = Email;
            this.Password = Password;
        }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(10, MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
