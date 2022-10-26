using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class WithdrawFundValidator: AbstractValidator<WithdrawFund>
    {
        public WithdrawFundValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(long.MinValue);

            RuleFor(x => x.Amount)
                .GreaterThan(0);
        }
    }
}
