using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Interface.IServices;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;

namespace Savemate.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        // 🔹 Add new transaction
        public async Task<Transaction> AddTransactionAsync(Transaction transaction, string userId, CancellationToken ct = default)
        {
            // Validate account(s)
            if (transaction.FromAccountId.HasValue)
            {
                var fromAccount = await _accountRepository.GetByIdAsync(transaction.FromAccountId.Value, userId, ct);
                if (fromAccount == null)
                    throw new InvalidOperationException("Invalid source account.");
            }

            if (transaction.ToAccountId.HasValue)
            {
                var toAccount = await _accountRepository.GetByIdAsync(transaction.ToAccountId.Value, userId, ct);
                if (toAccount == null)
                    throw new InvalidOperationException("Invalid destination account.");
            }

            return await _transactionRepository.AddAsync(transaction, userId, ct);
        }

        // 🔹 Update transaction
        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction, string userId, CancellationToken ct = default)
        {
            return await _transactionRepository.UpdateAsync(transaction, userId, ct);
        }

        // 🔹 Delete transaction
        public async Task<bool> DeleteTransactionAsync(int id, string userId, CancellationToken ct = default)
        {
            return await _transactionRepository.DeleteAsync(id, userId, ct);
        }

        // 🔹 Get single transaction
        public async Task<Transaction?> GetTransactionByIdAsync(int id, string userId, CancellationToken ct = default)
        {
            return await _transactionRepository.GetByIdAsync(id, userId, ct);
        }

        // 🔹 Get transactions by user (with filters + pagination)
        public async Task<IReadOnlyList<Transaction>> ListTransactionsByUserAsync(
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
            return await _transactionRepository.ListByUserAsync(
                userId, from, to, accountId, categoryId, type, skip, take, ct);
        }

        // 🔹 Get sum of transactions (e.g., total expenses, total income)
        public async Task<decimal> GetTransactionSumAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            int? accountId = null,
            TransactionTypeEnum? type = null,
            CancellationToken ct = default)
        {
            return await _transactionRepository.SumAsync(userId, from, to, accountId, type, ct);
        }
    }
}
