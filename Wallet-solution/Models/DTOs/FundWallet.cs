using System.ComponentModel.DataAnnotations;

namespace Wallet_solution.Models.DTOs
{
    public class FundWallet
    {
        public FundWallet(long AccountNumber, decimal Amount)
        {
            this.AccountNumber = AccountNumber;
            this.Amount = Amount;
        }

        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
