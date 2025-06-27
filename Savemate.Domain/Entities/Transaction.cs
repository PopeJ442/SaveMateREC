using Savemate.Domain.Enums;
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
        public Guid UserId { get; set; }

        public int? FromAccountId { get; set; }  // nullable for income
        public int? ToAccountId { get; set; }    // nullable for expenses
        public decimal Amount { get; set; }

        public TransactionTypeEnum Type { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public int? CategoryId { get; set; }

        public User User { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
        public Category Category { get; set; }
    }
}
