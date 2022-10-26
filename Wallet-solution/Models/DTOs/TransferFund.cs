using System.ComponentModel.DataAnnotations;

namespace Wallet_solution.Models.DTOs
{
    public class TransferFund
    {
        public TransferFund(long AccountNumber, long RecipientAccountNumber, decimal Amount)
        {
            this.AccountNumber = AccountNumber;
            this.Amount = Amount;
            this.RecipientAccountNumber = RecipientAccountNumber;
        }

        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public long RecipientAccountNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
