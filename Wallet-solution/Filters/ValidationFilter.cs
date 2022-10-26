using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wallet_solution.Models;

namespace Wallet_solution.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly WalletDbContext _dbContext;

        public ValidationFilter(WalletDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                var errorResponse = new ErrorResponse();

                foreach(var error in errorsInModelState)
                {
                    foreach(var subErr in error.Value)
                    {
                        errorResponse.Errors.Add(new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subErr
                        });
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }


    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }

    public class ErrorModel
    {
        public string? FieldName { get; set; }

        public string? Message { get; set; }
    }
}
