using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_solution.Models
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public TransactionType TransactionType { get; set; }

        [Column(TypeName = "decimal(38,2)")]
        public decimal TransactionAmount { get; set; }

        [ForeignKey(nameof(User))]

        public Guid UserId { get; set; }

        public virtual User? User { get; set; }

        public long AccountNumber { get; set; }

    }

    public enum TransactionType
    {
        CREDIT = 0,
        DEBIT = 1
    }
}
