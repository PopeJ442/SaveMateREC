
using Savemate.Domain.Entities;
 

namespace Savemate.Application.Interfaces.Services
{

    public interface ITransactionService
    {
        Task<Transaction?> GetTransactionAsync(int id, string userId);
        Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId);
        Task<(bool IsSuccess, string Message, Transaction? Result)> CreateTransactionAsync(Transaction transaction );
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id, string userId);
        Task<(bool IsSuccess, string Message)> ReverseTransactionAsync(int id, string userId);

    }
}
