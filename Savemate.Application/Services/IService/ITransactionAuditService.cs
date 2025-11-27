using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services.IService
{
    public interface ITransactionAuditService
    {
        Task LogAsync(TransactionAuditLog auditEntry);
        Task<IEnumerable<TransactionAuditLog>> GetHistoryAsync(int transactionId);
    }

}
