using MediatR;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.CommandHandlers.UserHandlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ResponseModel>
    {

        private readonly AuthenticationService _authService;
        private readonly WalletDbContext _dbContext;

        public UserLoginCommandHandler(AuthenticationService _authService, WalletDbContext _dbContext)
        {
            this._authService = _authService;
            this._dbContext = _dbContext;
        }

        public async Task<ResponseModel> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var result = await Task.Run(() =>
            {
                if (_authService.VerifyPasswordHash(request))
                    return _authService.CreateToken(request);

                return $"Error trying to Login user.";
            });

            if (result.Contains("Error")) return new ResponseModel { Success = false, ErrorMessage = result };

            return new ResponseModel
            {
                Success = true,
                ErrorMessage = string.Empty,
                Data = result
            };
        }
    }
}
