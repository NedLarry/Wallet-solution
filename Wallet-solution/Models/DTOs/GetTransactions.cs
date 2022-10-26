namespace Wallet_solution.Models.DTOs
{
    public class GetTransactions
    {
        public GetTransactions(Guid Id)
        {
            this.userId = Id;
        }

        public Guid userId { get; set; }
    }

    public class GetTransactionForWallet
    {
        public GetTransactionForWallet(long AccountNumber)
        {
            this.AccountNumber = AccountNumber;
        }

        public long AccountNumber { get; set; }
    }
}
