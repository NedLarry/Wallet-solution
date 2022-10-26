using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;

namespace Wallet_solution.Services
{
    public class WalletService
    {
        private readonly WalletDbContext _dbContext;

        public WalletService(WalletDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<List<WalletView>> CreateWalletForUser (User user)
        {
            List<WalletView> userWallets = new List<WalletView> ();
            try
            {
                var nairaWallet = new Wallet
                {
                    UserId = user.Id,
                    Balance = 0.0M,
                    WalletType = AccountType.NAIRA,
                    AccountNumber = CreateAccountNumber(),
                };
                
                var dollarWallet = new Wallet
                {
                    UserId = user.Id,
                    Balance = 0.0M,
                    WalletType = AccountType.DOLLAR,
                    AccountNumber = CreateAccountNumber(),
                };

                _dbContext.Wallets.AddRange(dollarWallet, nairaWallet);

                await _dbContext.SaveChangesAsync();

                userWallets.AddRange(new List<WalletView>
                {
                    new WalletView
                    {
                        WalletId = dollarWallet.Id,
                        WalletType = dollarWallet.WalletType.ToString(),
                        AccountNumber = dollarWallet.AccountNumber,
                        Balance = dollarWallet.Balance,
                    },

                    new WalletView
                    {
                        WalletId = nairaWallet.Id,
                        WalletType = nairaWallet.WalletType.ToString(),
                        AccountNumber = nairaWallet.AccountNumber,
                        Balance = nairaWallet.Balance,
                    }

                });

            }catch (Exception ex)
            {
                return null;
            }

            return userWallets;

            long CreateAccountNumber()
            {
                int _loopLimit = 10, counter = 0, range = 4;
                string acc = "";

                while (counter < _loopLimit)
                {
                    acc += new Random().Next(range).ToString();
                    counter++;
                }

                return long.Parse(acc);

            }
        }

        public UserView GetWalletsForUser(GetUserWalletsQuery query)
        {
            try
            {
                List<WalletView> wallets = new List<WalletView>();

                User user = _dbContext.Users.First(u => u.Id.Equals(query.userId));

                List<Wallet> userWallets = _dbContext.Wallets.Where(w => w.UserId.Equals(user.Id)).ToList();

                userWallets.ForEach(w => wallets.Add(new WalletView
                {
                    AccountNumber = w.AccountNumber,
                    Balance = w.Balance,
                    WalletId = w.Id,
                    WalletType = w.WalletType.ToString()
                }));

                return new UserView { userId = user.Id, Fullname = String.Join(" ", user.FirstName, user.LastName), Wallets = wallets };

            }catch(Exception ex)
            {
                return new UserView { Fullname = ex.Message };
            }
        }

        public string GetWalletBalance(GetWalletBalanceQuery query)
        {
            try
            {
                Wallet userWallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(query.AccountNumber));

                userWallet.User = _dbContext.Users.First(u => u.Id.Equals(userWallet.UserId));

                return $"Account summary\n{userWallet}";

            }catch(Exception ex)
            {
                return $"Error getting wallet balance with accountNumber: {query.AccountNumber}\nError: {ex.Message}";
            }
        }

        public List<Wallet> GetAllWallets() => _dbContext.Wallets.ToList();
    }
}
