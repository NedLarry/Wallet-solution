using MediatR;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.QueryHandlers.UserQueryHandlers
{
    public class GetUserWalletQueryHandler : IRequestHandler<GetUserWalletsQuery, UserView>
    {
        private readonly WalletService _walletService;

        public GetUserWalletQueryHandler(WalletService _walletService)
        {
            this._walletService = _walletService;
        }
        public async Task<UserView> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
        {
            var result = await Task.Run(() =>
            {
                return _walletService.GetWalletsForUser(request);
            });

            return result;
        }
    }
}
