namespace Wallet_solution.Models.DTOs
{
    public class UserCreated
    {
        public string? Fullname { get; set; }

        public List<WalletView> Wallets { get; set; } = new List<WalletView>();

        public string? Error { get; set; }
    }
}
