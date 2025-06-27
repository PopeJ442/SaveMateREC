using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        
        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }
      
        public string? LastName { get; set; }
        
        public DateOnly DOB { get; set; }

        public ICollection<Account> Accounts { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];
        public ICollection<Category> Categories { get; set; } = [];

    }
}
