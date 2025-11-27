using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Interface.IRepositories
{
    public interface ITransactionAuditRepository
    {
        Task AddAsync(TransactionAuditLog audit);
        Task<IEnumerable<TransactionAuditLog>> GetByTransactionIdAsync(int transactionId);
        Task<IEnumerable<TransactionAuditLog>> GetAllAsync();
        Task SaveChangesAsync();
    }

}
