using Savemate.Domain.Entities;
using Savemate.Domain.Enums;

namespace Savemate.Application.Interface.IServices
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransactionAsync(Transaction transaction, string userId, CancellationToken ct = default);

        Task<Transaction?> UpdateTransactionAsync(Transaction transaction, string userId, CancellationToken ct = default);

        Task<bool> DeleteTransactionAsync(int id, string userId, CancellationToken ct = default);

        Task<Transaction?> GetTransactionByIdAsync(int id, string userId, CancellationToken ct = default);

        Task<IReadOnlyList<Transaction>> ListTransactionsByUserAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            int? categoryId = null,
            TransactionTypeEnum? type = null,
            int skip = 0,
            int take = 100,
            CancellationToken ct = default);

        Task<decimal> GetTransactionSumAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            TransactionTypeEnum? type = null,
            CancellationToken ct = default);
    }
}
