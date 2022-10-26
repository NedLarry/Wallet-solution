using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class TransferFundValidator : AbstractValidator<TransferFund>
    {
        public TransferFundValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(long.MinValue);

            RuleFor(x => x.RecipientAccountNumber)
                .GreaterThan(long.MinValue);

            RuleFor(x => x.Amount)
                .GreaterThan(0);
        }
    }
}
