namespace Wallet_solution.Models.DTOs
{
    public class UserView
    {
        public Guid userId { get; set; }
        public string? Fullname { get; set; }

        public List<WalletView> Wallets { get; set; } = new List<WalletView>();
    }

    public class UserTransactionView
    {
        public Guid userId { get; set; }

        public string? Fullname { get; set; }

        public List<TransactionView> Transactions { get; set; } = new List<TransactionView>();

        public decimal CurrentWalletBalance { get; set; }
    }

    public class UserTransactions
    {
        public Guid userId { get; set; }

        public string? Fullname { get; set; }

        public List<TransactionView> Transactions { get; set; } = new List<TransactionView>();

    }
}
