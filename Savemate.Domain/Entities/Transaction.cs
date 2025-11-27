using Microsoft.EntityFrameworkCore;
using Savemate.Domain.Enums;
using Savemate.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;


namespace Savemate.Domain.Entities
{
    public class Transaction
    { 
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 
 
        public int? FromAccountId { get; set; }   
        public Account? FromAccount { get; set; }

        
        public int? ToAccountId { get; set; }     
        public Account? ToAccount { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum Type { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        public bool IsReversed { get; set; } = false;
        public bool IsReversalEntry { get; set; } = false;

        
        public int? ParentTransactionId { get; set; }
        public Transaction? ParentTransaction { get; set; }


    }
}
