﻿using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{
    public class FundWalletCommandHandler : IRequestHandler<FundWalletCommand, string>
    {
        private readonly UserService _userService;

        public FundWalletCommandHandler(UserService _userService)
        {
            this._userService = _userService;
        }
        public async Task<string> Handle(FundWalletCommand request, CancellationToken cancellationToken)
        {
            return await _userService.FundWallet(request);
        }
    }
}
