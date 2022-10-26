using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;

namespace Wallet_solution.Services
{
    public class TransactionService
    {
        private readonly WalletDbContext _dbContext;

        public TransactionService(WalletDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }


        public UserTransactions GetTransactionForUser(GetUserTransactionsQuery query)
        {
            try
            {
                List<TransactionView> transactions = new List<TransactionView>();

                User user = _dbContext.Users.First(u => u.Id.Equals(query.userId));

                List<Transaction> userTransactions = _dbContext.Transactions.Where(t => t.UserId.Equals(user.Id)).ToList();

                userTransactions.ForEach(t => transactions.Add(new TransactionView
                {
                    UserId = t.UserId,
                    AccountNumber = t.AccountNumber,
                    Amount = t.TransactionAmount,
                    TransactionType = t.TransactionType.ToString()
                }));

                return new UserTransactions
                {
                    userId = user.Id,
                    Fullname = string.Join(" ", user.FirstName, user.LastName),
                    Transactions = transactions,
                };

            }catch (Exception ex)
            {
                var message = $"Error fetching transactions for user: {query.userId}";
                return new UserTransactions { Fullname = $"{message}\nError: {ex.Message}" };
            }
        }

        public UserTransactionView GetTransactionForWallet(GetTransactionForWalletQuery query)
        {
            try
            {
                List<TransactionView> transactions = new List<TransactionView>();

                Wallet wallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(query.AccountNumber));

                User user = _dbContext.Users.First(u => u.Id.Equals(wallet.UserId));

                List<Transaction> walletTransactions = _dbContext.Transactions.Where(t => t.AccountNumber.Equals(wallet.AccountNumber)).ToList();

                walletTransactions.ForEach(t => transactions.Add(new TransactionView
                {
                    TransactionType = t.TransactionType.ToString(),
                    AccountNumber = t.AccountNumber,
                    Amount = t.TransactionAmount,
                    UserId = user.Id

                }));

                decimal creditTotal = transactions.Where(t => t.TransactionType.Equals(TransactionType.CREDIT.ToString()))
                    .Sum(t => t.Amount);

                decimal debitTotal = transactions.Where(t => t.TransactionType.Equals(TransactionType.DEBIT.ToString()))
                    .Sum(t => t.Amount);

                return new UserTransactionView
                {
                    userId = user.Id,
                    Fullname = string.Join(" ", user.FirstName, user.LastName),
                    Transactions = transactions,
                    CurrentWalletBalance = (creditTotal - debitTotal)
                };

            }catch (Exception ex)
            {
                var message = $"Error fetching transactions for wallet: {query.AccountNumber}";
                return new UserTransactionView { Fullname = $"{message}\nError: {ex.Message}" };
            }
        }
    }
}
