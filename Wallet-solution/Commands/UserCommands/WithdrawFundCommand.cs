using MediatR;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Commands.UserCommands
{
    public class WithdrawFundCommand : IRequest<ResponseModel>
    {
        public WithdrawFundCommand(long AccountNumber, decimal Amount)
        {
            this.AccountNumber = AccountNumber;
            this.Amount = Amount;
        }


        public long AccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
