using MediatR;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.QueryHandlers.UserQueryHandlers
{
    public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, ResponseModel>
    {
        private readonly WalletService _walletService;

        public GetWalletBalanceQueryHandler(WalletService _walletService)
        {
            this._walletService = _walletService;
        }

        public async Task<ResponseModel> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            var result = await Task.Run(() =>
            {
                return _walletService.GetWalletBalance(request);
            });

            return result;
        }
    }
}
