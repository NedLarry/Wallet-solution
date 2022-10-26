using Microsoft.EntityFrameworkCore;

namespace Wallet_solution.Models
{
    public class WalletDbContext: DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options): base(options)
        {
                
        }


        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Wallet>()
            //    .HasOne(w => w.User)
            //    .WithMany(u => u.Wallets);

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(T => T.User)
            //    .WithMany(U => U.Transactions);

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Transactions)
            //    .WithOne();

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Wallets)
            //    .WithOne()
            //    .HasPrincipalKey(u => u.Id);
        }
    }
}
