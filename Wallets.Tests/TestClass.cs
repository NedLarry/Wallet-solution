using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;

namespace Wallets.Tests
{
    [TestClass]
    public class TestClass
    {

        private readonly HttpClient _client;

        public TestClass()
        {
            var appfactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(WalletDbContext));
                        services.AddDbContext<WalletDbContext>(opts =>
                        {
                            opts.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            _client = appfactory.CreateClient();
        }


        protected async Task Authenticate()
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetJwt());


            async Task<string> GetJwt()
            {
                var createdUserReq = await _client.PostAsJsonAsync("/user/register", new CreateUserCommand
                {
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "Admin.admin@admin.com",
                    Password = "Admin1234"
                });

                var userCreatedResponse = JsonConvert.DeserializeObject<UserCreated>(await createdUserReq.Content.ReadAsStringAsync());

                var userJwtReq = await _client.PostAsJsonAsync("/user/login", new UserLoginCommand
                {
                    Email = "Admin.admin@admin.com",
                    Password = "Admin1234"
                });

                return await userJwtReq.Content.ReadAsStringAsync();
            }
        }
    }
}