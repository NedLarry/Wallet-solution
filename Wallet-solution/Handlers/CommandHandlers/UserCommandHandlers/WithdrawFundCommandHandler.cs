using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{
    public class WithdrawFundCommandHandler : IRequestHandler<WithdrawFundCommand, string>
    {
        private readonly UserService _userService;

        public WithdrawFundCommandHandler(UserService _userService)
        {
            this._userService = _userService;
        }
        public async Task<string> Handle(WithdrawFundCommand request, CancellationToken cancellationToken)
        {
            return await _userService.WithdrawFund(request);
        }
    }
}
