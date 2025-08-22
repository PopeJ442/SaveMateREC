using Savemate.Domain.Entities;
using Savemate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Savemate.Application.Interface.IRepositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<Transaction?> GetByIdAsync(int id, string userId, CancellationToken ct = default);

        Task<IReadOnlyList<Transaction>> ListByUserAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            int? categoryId = null,
            TransactionTypeEnum? type = null,
            int skip = 0,
            int take = 100,
            CancellationToken ct = default);

        Task<decimal> SumAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            TransactionTypeEnum? type = null,
            CancellationToken ct = default);

        Task<Transaction> AddAsync(Transaction transaction, string userId, CancellationToken ct = default);
        Task<Transaction?> UpdateAsync(Transaction transaction, string userId, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, string userId, CancellationToken ct = default);
    }
}
