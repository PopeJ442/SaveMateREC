using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; 
using Savemate.Domain.Entities;
 
namespace Savemate.Infrastructure
{

    public class SaveMateDbContext(DbContextOptions<SaveMateDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransactionAuditLog> AuditLog { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Account (1:M)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
       .HasOne(t => t.FromAccount)
       .WithMany(a => a.TransactionsFrom)
       .HasForeignKey(t => t.FromAccountId)
       .OnDelete(DeleteBehavior.Restrict); // Prevent cascade loop

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.TransactionsTo)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
    .HasOne(t => t.ParentTransaction)
    .WithMany()
    .HasForeignKey(t => t.ParentTransactionId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionAuditLog>()
            .HasOne(a => a.Transaction)
            .WithMany()
            .HasForeignKey(a => a.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

            // User → Category (1:M)
            modelBuilder.Entity<Category>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

           

        }

    }
}
