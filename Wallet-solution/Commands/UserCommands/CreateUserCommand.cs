using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Commands.UserCommands
{
    public class CreateUserCommand: IRequest<UserCreated>
    {
        //Fluentvalidation needed
        public CreateUserCommand(string FirstName, string LastName, string Email, string password)
        {
           

            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Password = password;

        }

        public CreateUserCommand()
        {

        }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [EmailAddress, Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
