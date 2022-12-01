using MediatR;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Queries.UserQuery
{
    public class GetUserTransactionsQuery: IRequest<ResponseModel>
    {
        public GetUserTransactionsQuery(Guid Id)
        {
            this.userId = Id;
        }

        public Guid userId { get; set; }
    }


    public class GetTransactionForWalletQuery : IRequest<ResponseModel>
    {

        public GetTransactionForWalletQuery(long AccountNumber)
        {
            this.AccountNumber = AccountNumber;
        }

        public long AccountNumber { get; set; }
    }
}
