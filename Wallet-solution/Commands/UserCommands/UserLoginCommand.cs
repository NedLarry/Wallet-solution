using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Commands.UserCommands
{
    public class UserLoginCommand: IRequest<ResponseModel>
    {
        public UserLoginCommand(string Email, string Password)
        {

            this.Email = Email;
            this.Password = Password;
        }

        public UserLoginCommand()
        {

        }
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
