using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserCreated>
    {

        private readonly UserService _userService;
        private readonly WalletService _walletService;
        private readonly WalletDbContext _dbContext;

        public CreateUserCommandHandler(UserService _userService, WalletDbContext _dbContext, WalletService _walletService)
        {
            this._userService = _userService;
            this._dbContext = _dbContext;
            this._walletService = _walletService;
        }

        public async Task<UserCreated> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var newUser = await _userService.RegisterUser(request);

            if (newUser is null) return new UserCreated { Error = $"Email: {request.Email} is already in use." };

            var userWallets = await _walletService.CreateWalletForUser(newUser);

            return new UserCreated { Fullname = String.Join(" ", newUser.FirstName, newUser.LastName), Email = newUser.Email, Wallets = userWallets };
        }
    }
}
