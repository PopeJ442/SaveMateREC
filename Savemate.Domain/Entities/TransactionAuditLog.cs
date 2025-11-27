using Savemate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Entities
{
    public class TransactionAuditLog
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public AuditTypeEnum AuditType { get; set; }  

        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }

        public int? OldFromAccountId { get; set; }
        public int? NewFromAccountId { get; set; }

        public int? OldToAccountId { get; set; }
        public int? NewToAccountId { get; set; }

        public DateTime OldDate { get; set; }
        public DateTime NewDate { get; set; }

        public string? OldNote { get; set; }
        public string? NewNote { get; set; }

        public DateTime ChangedOn { get; set; }
        public string? ChangedByUserId { get; set; }
    }

}
