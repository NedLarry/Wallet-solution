using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class GetWalletsValidator: AbstractValidator<GetUserWallets>
    {
        public GetWalletsValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty();
        }
    }

    public class GetWalletBalanceValidator : AbstractValidator<GetWalletBalance>
    {
        public GetWalletBalanceValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(long.MinValue);
        }
    }

}
