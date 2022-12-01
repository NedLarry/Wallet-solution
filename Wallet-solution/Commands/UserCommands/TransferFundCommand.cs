using MediatR;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Commands.UserCommands
{
    public class TransferFundCommand : IRequest<ResponseModel>
    {
        public TransferFundCommand(long AccountNumber, long RecipientAccountNumber, decimal Amount)
        {
            this.AccountNumber = AccountNumber;
            this.Amount = Amount;
            this.RecipientAccountNumber = RecipientAccountNumber;
        }


        public long AccountNumber { get; set; }

        public long RecipientAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
