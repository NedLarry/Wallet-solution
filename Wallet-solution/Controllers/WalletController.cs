using MediatR;
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
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Backgroundjob _backJob;
        private readonly WalletDbContext _context;

        public WalletController(IMediator _mediator, Backgroundjob _backJob, WalletDbContext _context)
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
            await Task.Run(() => _backJob.ScheduleInterestJob());
            return Ok("Background service started");
        }
    }
}
