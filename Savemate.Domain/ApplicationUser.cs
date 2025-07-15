using Microsoft.AspNetCore.Identity;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public   string LastName { get; set; }
        public DateOnly DOB { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual  ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual  ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
