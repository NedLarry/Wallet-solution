using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet_solution.Filters;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;

namespace Wallet_solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpGet("user-transactions")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> GetTransactionsForUser([FromQuery] Guid userId)
        {
            var result = await _mediator.Send(new GetUserTransactionsQuery(userId));
            return Ok(result);
        }

        [HttpGet("user-wallet-transaction")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult> GetTransactioinForWallet([FromQuery] long accountNumber)
        {
            var result = await _mediator.Send(new GetTransactionForWalletQuery(accountNumber));
            return Ok(result);
        }
    }
}
