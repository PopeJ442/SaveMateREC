namespace Savemate.Application.Interfaces.Repositories
{
    using Savemate.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}
