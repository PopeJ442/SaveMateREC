using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interfaces.Repositories;
using Savemate.Domain.Entities;
 

namespace Savemate.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SaveMateDbContext _context;

        public TransactionRepository(SaveMateDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
              //  .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
               
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
