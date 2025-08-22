using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;

namespace Savemate.Infrastructure.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly SaveMateDbContext _db;

        public TransactionRepository(SaveMateDbContext context) : base(context)
        {
            _db = context;
        }

        // 🔹 Get single transaction by id & user
        public async Task<Transaction?> GetByIdAsync(int id, string userId, CancellationToken ct = default)
        {
            return await _db.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Category)
                .Where(t => t.Id == id && t.UserId.ToString() == userId)
                .FirstOrDefaultAsync(ct);
        }

        // 🔹 Get transactions by user with filters + pagination
        public async Task<IReadOnlyList<Transaction>> ListByUserAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            int? categoryId = null,
            TransactionTypeEnum? type = null,
            int skip = 0,
            int take = 100,
            CancellationToken ct = default)
        {
            var query = _db.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Category)
                .Where(t => t.UserId.ToString() == userId);

            if (from.HasValue) query = query.Where(t => t.Date >= from.Value);
            if (to.HasValue) query = query.Where(t => t.Date <= to.Value);
            if (accountId.HasValue)
                query = query.Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId);
            if (categoryId.HasValue) query = query.Where(t => t.CategoryId == categoryId);
            if (type.HasValue) query = query.Where(t => t.Type == type);

            return await query
                .OrderByDescending(t => t.Date)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }

        // 🔹 Sum transactions for reports/dashboards
        public async Task<decimal> SumAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            TransactionTypeEnum? type = null,
            CancellationToken ct = default)
        {
            var query = _db.Transactions.Where(t => t.UserId.ToString() == userId);

            if (from.HasValue) query = query.Where(t => t.Date >= from.Value);
            if (to.HasValue) query = query.Where(t => t.Date <= to.Value);
            if (accountId.HasValue)
                query = query.Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId);
            if (type.HasValue) query = query.Where(t => t.Type == type);

            return await query.SumAsync(t => t.Amount, ct);
        }

        // 🔹 Add transaction
        public async Task<Transaction> AddAsync(Transaction transaction, string userId, CancellationToken ct = default)
        {
            transaction.UserId = transaction.UserId.ToString( );
            await _db.Transactions.AddAsync(transaction, ct);
            await _db.SaveChangesAsync(ct);
            return transaction;
        }

        // 🔹 Update transaction
        public async Task<Transaction?> UpdateAsync(Transaction transaction, string userId, CancellationToken ct = default)
        {
            var existing = await _db.Transactions
                .FirstOrDefaultAsync(t => t.Id == transaction.Id && t.UserId.ToString() == userId, ct);

            if (existing == null) return null;

            existing.Amount = transaction.Amount;
            existing.Date = transaction.Date;
            existing.Note = transaction.Note;
            existing.Type = transaction.Type;
            existing.FromAccountId = transaction.FromAccountId;
            existing.ToAccountId = transaction.ToAccountId;
            existing.CategoryId = transaction.CategoryId;

            _db.Transactions.Update(existing);
            await _db.SaveChangesAsync(ct);

            return existing;
        }

        // 🔹 Delete transaction
        public async Task<bool> DeleteAsync(int id, string userId, CancellationToken ct = default)
        {
            var existing = await _db.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId.ToString() == userId, ct);

            if (existing == null) return false;

            _db.Transactions.Remove(existing);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
