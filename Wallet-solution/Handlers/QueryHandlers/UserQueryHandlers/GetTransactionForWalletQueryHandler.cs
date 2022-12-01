using MediatR;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.QueryHandlers.UserQueryHandlers
{
    public class GetTransactionForWalletQueryHandler : IRequestHandler<GetTransactionForWalletQuery, ResponseModel>
    {
        private readonly TransactionService _transactionService;

        public GetTransactionForWalletQueryHandler(TransactionService _transactionService)
        {
            this._transactionService = _transactionService;
        }
        public async Task<ResponseModel> Handle(GetTransactionForWalletQuery request, CancellationToken cancellationToken)
        {
            var result = await Task.Run(() =>
            {
                return _transactionService.GetTransactionForWallet(request);
            });

            return result;
        }
    }
}
