using FluentValidation;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Validators
{
    public class GetTransactionsValidator: AbstractValidator<GetTransactions>
    {
        public GetTransactionsValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty();
        }
    }

    public class GetTransactionsForAccountValidator : AbstractValidator<GetTransactionForWallet>
    {
        public GetTransactionsForAccountValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(long.MinValue);
        }
    }
}
