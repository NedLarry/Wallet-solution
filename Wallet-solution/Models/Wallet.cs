using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_solution.Models
{
    public class Wallet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public long AccountNumber { get; set; }

        [Required]
        public AccountType WalletType { get; set; }

        [Column(TypeName = "decimal(38,2)")]
        public decimal Balance { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual User? User { get; set; }

        public override string ToString()
        {
            string currency = WalletType.Equals(AccountType.DOLLAR) ? "$" : "#";

            return $"AccountName: {User.FirstName} {User.LastName}\n" +
                    $"AccountNumber: {AccountNumber}\n" +
                    $"Balance: {currency}{Balance}";
        }

        public string PrintCurrentAccountBalance()
        {
            string currency = WalletType.Equals(AccountType.DOLLAR) ? "$" : "#";
            return $"Balance: {currency}{Balance}";
        }
    }


    public enum AccountType
    {
        DOLLAR = 0,
        NAIRA = 1
    }
}
