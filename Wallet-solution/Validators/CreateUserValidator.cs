using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class CreateUserValidator : AbstractValidator<UserRegistration>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName)
        .NotEmpty();

            RuleFor(x => x.LastName)
                    .NotEmpty();

            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage($"Invalid email format");

            RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(6, 10)
                    .WithMessage($"Password length must be at least six characters long");
        }
    }
}
