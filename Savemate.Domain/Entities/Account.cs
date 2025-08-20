using Savemate.Domain.Enums;
using Savemate.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Entities
{
    public class Account
    {
            public int Id { get; set; }
            public string UserId { get; set; }
            public required string Name { get; set; }
            public AccountTypeEnum Type { get; set; }
            public decimal InitialBalance { get; set; }

            public ApplicationUser User { get; set; }
            public ICollection<Transaction> TransactionsFrom { get; set; } = [];
            public ICollection<Transaction> TransactionsTo { get; set; } = [];
        
    }
}
