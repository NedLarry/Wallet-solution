using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{
    public class TransferFundCommandHandler : IRequestHandler<TransferFundCommand, ResponseModel>
    {
        private readonly UserService _userService;

        public TransferFundCommandHandler(UserService _userService)
        {
            this._userService = _userService;
        }
        public async Task<ResponseModel> Handle(TransferFundCommand request, CancellationToken cancellationToken)
        {
            return await _userService.TransferFund(request);
        }
    }
}
