using Savemate.Domain.Enums;
using Savemate.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? FromAccountId { get; set; }  // nullable for income
        public Account FromAccount { get; set; }

        public Account ToAccount { get; set; }
        public int? ToAccountId { get; set; }    // nullable for expenses
      
        public decimal Amount { get; set; }
        public TransactionTypeEnum Type { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public int? CategoryId { get; set; }     
        public Category Category { get; set; }
    }
}
