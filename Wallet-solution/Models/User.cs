using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_solution.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? FirstName { get;set; }

        public string? LastName { get;set; }

        public string? Email { get;set; }

        public byte[] PasswordSalt { get;set; }

        public byte[] PasswordHash { get;set; }

        [NotMapped]
        public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();

        [NotMapped]
        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
