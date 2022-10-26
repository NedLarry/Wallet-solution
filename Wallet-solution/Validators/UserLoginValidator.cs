using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(6, 10)
                .WithMessage($"Password must be at least 6 characters long");
        }
    }
}
