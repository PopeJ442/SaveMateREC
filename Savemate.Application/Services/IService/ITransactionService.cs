namespace Savemate.Application.Interfaces.Services
{
    using Savemate.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionService
    {
        Task<Transaction?> GetTransactionAsync(int id, string userId);
        Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id, string userId);
    }
}
