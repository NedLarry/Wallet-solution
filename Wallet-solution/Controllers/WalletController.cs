using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet_solution.BackgroundWork;
using Wallet_solution.Filters;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;
using Wallet_solution.Services;

namespace Wallet_solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Backgroundjob _backJob;

        public WalletController(IMediator _mediator, Backgroundjob _backJob)
        {
            this._mediator = _mediator;
            this._backJob = _backJob;
        }


        [HttpGet("user-wallets")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> GetUserWallet([FromQuery] Guid userId)
        {
            var result = await _mediator.Send(new GetUserWalletsQuery (userId));
            return Ok(result);
        }

        [HttpGet("user-wallet-balance")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> GetWalletBalance([FromQuery] long accountNumber)
        {
            var result = await _mediator.Send(new GetWalletBalanceQuery(accountNumber));
            return Ok(result);
        }

        [HttpGet("give-interest")]
        public async Task<ActionResult> GiveThyPeopleMoney()
        {
            await Task.Run(() => _backJob.ProducerJob());

            await Task.Run(() => _backJob.ConsumerJob());

            return Ok("Background service started");
        }
    }
}
