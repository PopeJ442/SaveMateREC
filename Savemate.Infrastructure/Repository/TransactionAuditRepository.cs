using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
 

namespace Savemate.Infrastructure.Repository
{
    public class TransactionAuditRepository : ITransactionAuditRepository
    {
        private readonly SaveMateDbContext _context;

        public TransactionAuditRepository(SaveMateDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TransactionAuditLog audit)
        {
            await _context.AuditLog.AddAsync(audit);
        }

        public async Task<IEnumerable<TransactionAuditLog>> GetByTransactionIdAsync(int transactionId)
        {
            return await _context.AuditLog
                .Where(a => a.TransactionId == transactionId)
                .OrderByDescending(a => a.NewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionAuditLog>> GetAllAsync()
        {
            return await _context.AuditLog
                .OrderByDescending(a => a.NewDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
