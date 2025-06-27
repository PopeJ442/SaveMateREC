using Savemate.Domain.Enums;
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
            public Guid UserId { get; set; }
            public required string Name { get; set; }
            public AccountTypeEnum Type { get; set; }
            public decimal InitialBalance { get; set; }

            public User User { get; set; }
            public ICollection<Transaction> TransactionsFrom { get; set; } = [];
            public ICollection<Transaction> TransactionsTo { get; set; } = [];
        
    }
}
