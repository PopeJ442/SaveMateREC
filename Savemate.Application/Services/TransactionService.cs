namespace Savemate.Application.Services
{
    using Savemate.Application.Interface.IRepositories;
    using Savemate.Application.Interfaces.Repositories;
    using Savemate.Application.Interfaces.Services;
    using Savemate.Domain.Entities;
    using Savemate.Domain.Enums;
    using System;
    using System.Threading.Tasks;

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

        public async Task<Transaction?> GetTransactionAsync(int id, string userId)
        {
            var tx = await _transactionRepository.GetByIdAsync(id);
            return (tx != null && tx.UserId == userId) ? tx : null;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId)
        {
            return await _transactionRepository.GetByUserIdAsync(userId);
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            // Fetch accounts if applicable
            var fromAccount = transaction.FromAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(transaction.FromAccountId.Value)
                : null;

            var toAccount = transaction.ToAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(transaction.ToAccountId.Value)
                : null;

            // Apply business rules
            switch (transaction.Type)
            {
                case TransactionTypeEnum.Income:
                    if (toAccount == null)
                        throw new InvalidOperationException("Income must have a ToAccount.");
                    toAccount.InitialBalance += transaction.Amount;
                    break;

                case TransactionTypeEnum.Expense:
                    if (fromAccount == null)
                        throw new InvalidOperationException("Expense must have a FromAccount.");
                    if (fromAccount.InitialBalance < transaction.Amount)
                        throw new InvalidOperationException("Insufficient funds.");
                    fromAccount.InitialBalance -= transaction.Amount;
                    break;

                case TransactionTypeEnum.Transfer:
                    if (fromAccount == null || toAccount == null)
                        throw new InvalidOperationException("Transfer must have both accounts.");
                    if (fromAccount.InitialBalance < transaction.Amount)
                        throw new InvalidOperationException("Insufficient funds.");
                    fromAccount.InitialBalance -= transaction.Amount;
                    toAccount.InitialBalance += transaction.Amount;
                    break;
            }

            // Persist
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return transaction;
        }

        public async Task DeleteTransactionAsync(int id, string userId)
        {
            var tx = await GetTransactionAsync(id, userId);
            if (tx == null)
                throw new InvalidOperationException("Transaction not found or unauthorized.");

            await _transactionRepository.DeleteAsync(tx);
            await _transactionRepository.SaveChangesAsync();
        }
    }
}
