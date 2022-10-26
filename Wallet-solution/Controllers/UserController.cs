using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Filters;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;

namespace Wallet_solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegistration createUser)
        {
            var result = await _mediator
                .Send
                (new CreateUserCommand(createUser.FirstName, createUser.LastName, createUser.Email, createUser.Password));

            return Ok(result);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> AuthenticateUser([FromBody] UserLogin loginUser)
        {
            var result = await _mediator.Send(new UserLoginCommand(loginUser.Email, loginUser.Password));
            return Ok(result);
        }

        [HttpPost("fund-wallet")]
        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize]
        public async Task<ActionResult> FundWallet([FromBody] FundWallet fundwallet)
        {
            var result = await _mediator.Send(new FundWalletCommand(fundwallet.AccountNumber, fundwallet.Amount));

            return Ok(result);
        }

        [HttpPost("withdraw-fund")]
        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize]
        public async Task<ActionResult> WithdrawFund([FromBody] WithdrawFund withdrawfund)
        {
            var result = await _mediator.Send(new WithdrawFundCommand(withdrawfund.AccountNumber, withdrawfund.Amount));

            return Ok(result);
        }

        [HttpPost("transfer-fund")]
        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize]
        public async Task<ActionResult> TransferFund([FromBody] TransferFund transferFund)
        {
            var result = await _mediator
                .Send
                (new TransferFundCommand(transferFund.AccountNumber, transferFund.RecipientAccountNumber, transferFund.Amount));

            return Ok(result);
        }
    }
}
