using MediatR;

namespace Wallet_solution.Commands.UserCommands
{
    public class FundWalletCommand : IRequest<string>
    {
        public FundWalletCommand(long AccountNumber, decimal Amount)
        {
            this.AccountNumber = AccountNumber;
            this.Amount = Amount;
        }


        public long AccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
