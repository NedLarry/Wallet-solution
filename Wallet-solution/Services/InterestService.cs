using Microsoft.EntityFrameworkCore;
using Wallet_solution.Models;

namespace Wallet_solution.Services
{
    public class InterestService
    {
        private readonly IServiceScopeFactory _factory;


        public InterestService(IServiceScopeFactory _factory)
        {
            this._factory = _factory;
        }

        public async Task<int> WalletYealyInterest()
        {

            int TotalWalletCount = 0;

            using (var scope = _factory.CreateScope())
            {
                WalletDbContext context = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

                List<Wallet> wallets = await context.Wallets.ToListAsync();

                if (wallets.Any())
                {
                    foreach (Wallet wallet in wallets)
                    {
                        decimal currentBalance = wallet.Balance;

                        decimal interest = ((currentBalance * 10 * 1) / 100);

                        wallet.Balance += interest;

                        context.Wallets.Update(wallet);

                        context.Transactions.Add(new Transaction
                        {
                            UserId = wallet.UserId,
                            TransactionAmount = interest,
                            TransactionType = TransactionType.CREDIT,
                            AccountNumber = wallet.AccountNumber,
                        });
                    }

                    await context.SaveChangesAsync();

                    TotalWalletCount = wallets.Count();
                }
            }


            return TotalWalletCount;

        }
    }
}
