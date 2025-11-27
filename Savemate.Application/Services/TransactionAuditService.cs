using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services
{
    public class TransactionAuditService : ITransactionAuditService
    {
        private readonly ITransactionAuditRepository _auditRepository;

        public TransactionAuditService(ITransactionAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task LogAsync(TransactionAuditLog auditEntry)
        {
            await _auditRepository.AddAsync(auditEntry);
        }

        public async Task<IEnumerable<TransactionAuditLog>> GetHistoryAsync(int transactionId)
        {
            return await _auditRepository.GetByTransactionIdAsync(transactionId);
        }
    }

}
