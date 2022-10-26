﻿using MediatR;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;
using Wallet_solution.Services;

namespace Wallet_solution.Handlers.QueryHandlers.UserQueryHandlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUserQuery, List<UserView>>
    {
        private readonly UserService _userService;

        public GetUsersQueryHandler(UserService _userService)
        {
            this._userService = _userService;
        }
        public async Task<List<UserView>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => _userService.GetUsers(request));
        }
    }
}
