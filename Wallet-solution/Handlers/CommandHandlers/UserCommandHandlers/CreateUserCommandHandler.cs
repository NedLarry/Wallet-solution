using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel>
    {

        private readonly UserService _userService;
        private readonly WalletService _walletService;

        public CreateUserCommandHandler(UserService _userService,WalletService _walletService)
        {
            this._userService = _userService;
            this._walletService = _walletService;
        }

        //Refactor: user and wallet creation should be in one method.
        public async Task<ResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var newUser = await _userService.RegisterUser(request);

            if (newUser is null) return new ResponseModel { Success = false, ErrorMessage = $"Email: {request.Email} is already in use." };

            var userWallets = await _walletService.CreateWalletForUser(newUser);

            return new ResponseModel
            {
                Success = true,
                ErrorMessage = string.Empty,
                Data = new UserCreated 
                { 
                    Fullname = string.Join(" ", newUser.FirstName, newUser.LastName), 
                    Email = newUser.Email, 
                    Wallets = userWallets 
                }
            };

            //Todo: make it that if creating user wallet fails, user isn't created
        }
    }
}
