using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure
{
    
        public class SaveMateDbContext(DbContextOptions<SaveMateDbContext> options) : IdentityDbContext<ApplicationUser>(options)
        {
          
            public DbSet<Category> Categories { get; set; }
            public DbSet<Transaction> Transactions { get; set; }
            public DbSet<Account> Accounts { get; set; }
           



            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // User → Account (1:M)
                modelBuilder.Entity<Account>()
                    .HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // User → Transaction (1:M)
                modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // User → Category (1:M)
                modelBuilder.Entity<Category>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Transaction → FromAccount (M:1)
                modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.FromAccount)
                    .WithMany(a => a.TransactionsFrom)
                    .HasForeignKey(t => t.FromAccountId)
                    .OnDelete(DeleteBehavior.Restrict); // prevent circular cascade delete

                // Transaction → ToAccount (M:1)
                modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.ToAccount)
                    .WithMany(a => a.TransactionsTo)
                    .HasForeignKey(t => t.ToAccountId)
                    .OnDelete(DeleteBehavior.Restrict); // prevent circular cascade delete

                // Transaction → Category (M:1)
                modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // in case the category is deleted


            }
        
        }
}
