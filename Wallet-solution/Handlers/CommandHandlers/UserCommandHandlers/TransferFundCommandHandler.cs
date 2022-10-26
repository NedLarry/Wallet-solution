using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{
    public class TransferFundCommandHandler : IRequestHandler<TransferFundCommand, string>
    {
        private readonly UserService _userService;

        public TransferFundCommandHandler(UserService _userService)
        {
            this._userService = _userService;
        }
        public async Task<string> Handle(TransferFundCommand request, CancellationToken cancellationToken)
        {
            return await _userService.TransferFund(request);
        }
    }
}
