using MediatR;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Queries.UserQuery
{
    public class GetUserWalletsQuery : IRequest<UserView>
    {
        public GetUserWalletsQuery(Guid Id)
        {
            this.userId = Id;
        }

        public Guid userId { get; set; }   
    }

    public class GetWalletBalanceQuery : IRequest<string>
    {

        public GetWalletBalanceQuery(long AccountNumber)
        {
            this.AccountNumber = AccountNumber;
        }

        public long AccountNumber { get; set; }
    }
}
