using MediatR;
using Wallet_solution.Models.DTOs;

namespace Wallet_solution.Queries.UserQuery
{
    public class GetUserQuery : IRequest<List<UserView>>
    {
    }
}
