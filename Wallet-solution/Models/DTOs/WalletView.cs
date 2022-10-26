namespace Wallet_solution.Models.DTOs
{
    public class WalletView
    {
        public Guid WalletId { get; set; }
        public long AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public string? WalletType { get; set; }
    }

    public class TransactionView
    {
        public Guid UserId { get; set; }

        public long AccountNumber { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionType { get; set; }
    }
}
