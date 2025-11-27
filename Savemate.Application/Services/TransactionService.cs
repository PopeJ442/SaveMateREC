using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Interfaces.Repositories;
using Savemate.Application.Interfaces.Services;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;

namespace Savemate.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionAuditRepository _auditLogRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ITransactionAuditRepository auditLogRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _auditLogRepository = auditLogRepository;
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
            var fromAccount = transaction.FromAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(transaction.FromAccountId.Value)
                : null;

            var toAccount = transaction.ToAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(transaction.ToAccountId.Value)
                : null;

            switch (transaction.Type)
            {
                case TransactionTypeEnum.Income:
                    toAccount!.InitialBalance += transaction.Amount;
                    break;

                case TransactionTypeEnum.Expense:
                    fromAccount!.InitialBalance -= transaction.Amount;
                    break;

                case TransactionTypeEnum.Transfer:
                    fromAccount!.InitialBalance -= transaction.Amount;
                    toAccount!.InitialBalance += transaction.Amount;
                    break;
            }

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

       
        public async Task<(bool IsSuccess, string Message)> ReverseTransactionAsync(int id, string userId)
        {
            var originalTx = await GetTransactionAsync(id, userId);
            if (originalTx == null)
                return (false, "Transaction not found or unauthorized.");

            // Load accounts
            var oldFrom = originalTx.FromAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(originalTx.FromAccountId.Value)
                : null;

            var oldTo = originalTx.ToAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(originalTx.ToAccountId.Value)
                : null;

            // 1. REVERSE ACCOUNT BALANCES
            switch (originalTx.Type)
            {
                case TransactionTypeEnum.Income:
                    oldTo!.InitialBalance -= originalTx.Amount;
                    break;

                case TransactionTypeEnum.Expense:
                    oldFrom!.InitialBalance += originalTx.Amount;
                    break;

                case TransactionTypeEnum.Transfer:
                    oldTo!.InitialBalance -= originalTx.Amount;
                    oldFrom!.InitialBalance += originalTx.Amount;
                    break;
            }

            // 2. CREATE REVERSAL TRANSACTION
            var reversalTx = new Transaction
            {
                UserId = originalTx.UserId,
                Amount = originalTx.Amount,
                Date = DateTime.Now,
                Note = $"REVERSAL of #{originalTx.Id}",
                Type = originalTx.Type,

                // swap From <-> To
                FromAccountId = originalTx.ToAccountId,
                ToAccountId = originalTx.FromAccountId
            };

            await _transactionRepository.AddAsync(reversalTx);

            // 3. CREATE AUDIT LOG ENTRY
            var audit = new TransactionAuditLog
            {
                TransactionId = originalTx.Id,
                AuditType = AuditTypeEnum.Reversed,

                OldAmount = originalTx.Amount,
                NewAmount = originalTx.Amount,

                OldFromAccountId = originalTx.FromAccountId,
                NewFromAccountId = reversalTx.FromAccountId,

                OldToAccountId = originalTx.ToAccountId,
                NewToAccountId = reversalTx.ToAccountId,

                OldDate = originalTx.Date,
                NewDate = reversalTx.Date,

                OldNote = originalTx.Note,
                NewNote = reversalTx.Note
            };

            await _auditLogRepository.AddAsync(audit);

            // COMMIT ALL CHANGES
            await _transactionRepository.SaveChangesAsync();
            await _auditLogRepository.SaveChangesAsync();

            return (true, "Transaction reversed successfully");
        }
    }
}
