namespace Wallet_solution.Models.DTOs
{
    public class GetUserWallets
    {
        public GetUserWallets(Guid Id)
        {
            this.userId = Id;
        }

        public Guid userId { get; set; }
    }

    public class GetWalletBalance
    {
        public GetWalletBalance(long AccountNumber)
        {
            this.AccountNumber = AccountNumber;
        }

        public long AccountNumber { get; set; }
    }


}
