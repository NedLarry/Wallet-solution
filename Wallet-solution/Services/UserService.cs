using System.Security.Cryptography;
using System.Text;
using Wallet_solution.Commands.UserCommands;
using Wallet_solution.Models;
using Wallet_solution.Models.DTOs;
using Wallet_solution.Queries.UserQuery;

namespace Wallet_solution.Services
{
    public class UserService
    {
        private readonly WalletDbContext _dbContext;

        public UserService(WalletDbContext _dbContext)
        {
            this._dbContext = _dbContext;        
        }


        public async Task<User> RegisterUser(CreateUserCommand createUser)
        {
            try
            {

                if (_dbContext.Users.Any())
                {
                    if (_dbContext.Users.ToList().Any(u => u.Email.Equals(createUser.Email)))
                        return null;
                }

                CreatePasswordHash(createUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User newUser = new User()
                {
                    FirstName = createUser.FirstName,
                    LastName = createUser.LastName,
                    Email = createUser.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                _dbContext.Users.Add(newUser);

                await _dbContext.SaveChangesAsync();

                return newUser;

            }catch(Exception ex)
            {
                return null;
            }

            void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                using(var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
            }
        }

        public async Task<ResponseModel> FundWallet(FundWalletCommand fundWallet)
        {

            try
            {
                Wallet userWallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(fundWallet.AccountNumber));

                if (userWallet is null) return null;

                User walletUser = _dbContext.Users.First(u => u.Id.Equals(userWallet.UserId));

                userWallet.Balance += fundWallet.Amount;

                _dbContext.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.CREDIT,
                    TransactionAmount = fundWallet.Amount,
                    UserId = userWallet.UserId,
                    AccountNumber = fundWallet.AccountNumber
                    
                });

                _dbContext.Wallets.Update(userWallet);

                await _dbContext.SaveChangesAsync();

                return new ResponseModel
                {
                    Success = true,
                    ErrorMessage = string.Empty,
                    Data = new WalletBalanceView
                    {
                        AccountNumber = userWallet.AccountNumber,
                        AccountName = String.Join(" ", userWallet.User.FirstName, userWallet.User.LastName),
                        Balance = userWallet.WalletType.Equals(AccountType.DOLLAR) ? $"$ {userWallet.Balance}" : $"# {userWallet.Balance}"
                    }
                };

            }catch(Exception ex)
            {
                return new ResponseModel
                {
                    Success = false,
                    ErrorMessage = $"Error funding wallet with account number: {fundWallet.AccountNumber}"
                };
            }
        }

        public async Task<ResponseModel> WithdrawFund(WithdrawFundCommand withdrawFund)
        {
            try
            {
                Wallet userWallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(withdrawFund.AccountNumber));

                if (userWallet is null)
                    return new ResponseModel
                    {
                        Success = false,
                        ErrorMessage = $"Wallet with accountNumber: {withdrawFund.AccountNumber} not found."
                    };

                User walletUser = _dbContext.Users.First(u => u.Id.Equals(userWallet.UserId));

                if (withdrawFund.Amount > userWallet.Balance || userWallet.Balance == 0)
                    return new ResponseModel
                    {
                        Success = false,
                        ErrorMessage = $"Insufficient funds. Current balance for account: {withdrawFund.AccountNumber}\n{userWallet.PrintCurrentAccountBalance()}"
                    };

                userWallet.Balance -= withdrawFund.Amount;

                _dbContext.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.DEBIT,
                    TransactionAmount = withdrawFund.Amount,
                    UserId = walletUser.Id,
                    AccountNumber = withdrawFund.AccountNumber
                });

                _dbContext.Wallets.Update(userWallet);

                await _dbContext.SaveChangesAsync();

                return new ResponseModel
                {
                    Success = true,
                    Data = $"Your new Account Status\n{userWallet}"
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Success = false,
                    ErrorMessage = $"Error funding wallet with accountNumber: {withdrawFund.AccountNumber}.\nError: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> TransferFund(TransferFundCommand transferFund)
        {
            try
            {
                Wallet userWallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(transferFund.AccountNumber));

                if (userWallet is null)
                    return new ResponseModel { Success = false, ErrorMessage = $"Wallet with accountNumber: {transferFund.AccountNumber} not found." };

                User user = _dbContext.Users.First(u => u.Id.Equals(userWallet.UserId));

                Wallet recipientWallet = _dbContext.Wallets.First(w => w.AccountNumber.Equals(transferFund.RecipientAccountNumber));

                if (recipientWallet is null)
                    return new ResponseModel { Success = false, ErrorMessage = $"Recipient accountNumber: {transferFund.AccountNumber} not found." };

                User recipientUser = _dbContext.Users.First(u => u.Id.Equals(recipientWallet.UserId));

                if (transferFund.Amount > userWallet.Balance || userWallet.Balance == 0)
                    return new ResponseModel
                    {
                        Success = false,
                        ErrorMessage = $"Insufficient funds. Current balance for account: {transferFund.AccountNumber}. {userWallet.PrintCurrentAccountBalance()}"
                    };

                userWallet.Balance -= transferFund.Amount;

                recipientWallet.Balance += transferFund.Amount;

                _dbContext.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.DEBIT,
                    TransactionAmount = transferFund.Amount,
                    UserId = user.Id,
                    AccountNumber = transferFund.AccountNumber,
                });

                _dbContext.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.CREDIT,
                    TransactionAmount = transferFund.Amount,
                    UserId = recipientUser.Id,
                    AccountNumber = transferFund.RecipientAccountNumber
                });

                _dbContext.Wallets.UpdateRange(userWallet, recipientWallet);

                await _dbContext.SaveChangesAsync();

                return new ResponseModel { Success = true, Data = $"Your new Account Status\n{userWallet}" };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Success = false,
                    ErrorMessage = $"Error sending fund to wallet with accountNumber: {transferFund.RecipientAccountNumber}"
                };
            }
        }

        public async Task<ResponseModel> GetUsers(GetUserQuery query)
        {
            try
            {
                List<UserView> userViews = new List<UserView>();

                var users = _dbContext.Users.ToList();

                users.ForEach(u =>
                {
                    var userWallets = _dbContext.Wallets.Where(w => w.UserId.Equals(u.Id)).ToList();

                    List<WalletView> walletview = new List<WalletView>();

                    userWallets.ForEach(w =>
                    {
                        walletview.Add(new WalletView
                        {
                            AccountNumber = w.AccountNumber,
                            Balance = w.Balance,
                            WalletId = w.Id,
                            WalletType = w.WalletType.ToString()
                        });
                    });

                    userViews.Add(new UserView
                    {
                        userId = u.Id,
                        Fullname = string.Join(" ", u.FirstName, u.LastName),
                        Wallets = walletview
                    });
                });

                return new ResponseModel
                {
                    Success = true,
                    ErrorMessage = string.Empty,
                    Data = userViews
                };


            }catch(Exception ex)
            {
                return new ResponseModel { Success = false, ErrorMessage = $"Error fetching users.\nError: {ex.Message}" };
            }
        }
    }
}
