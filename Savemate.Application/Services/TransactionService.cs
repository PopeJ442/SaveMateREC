using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TransactionService> _logger;   
     

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ITransactionAuditRepository auditLogRepository,
            ILogger<TransactionService> logger )
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _auditLogRepository = auditLogRepository;
            _logger = logger;   
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

        public async Task<(bool IsSuccess, string Message, Transaction? Result)> CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                _logger.LogInformation("Starting a transaction");
                var fromAccount = transaction.FromAccountId.HasValue
                    ? await _accountRepository.GetByIdAsync(transaction.FromAccountId.Value)
                    : null;

                var toAccount = transaction.ToAccountId.HasValue
                    ? await _accountRepository.GetByIdAsync(transaction.ToAccountId.Value)
                    : null;

                // 🔥 VALIDATION: Check insufficient funds BEFORE creating transaction
                if (transaction.Type == TransactionTypeEnum.Expense ||
                    transaction.Type == TransactionTypeEnum.Transfer)
                {
                    if (fromAccount == null)
                        return (false, "Source account not found.", null);

                    if (fromAccount.InitialBalance < transaction.Amount)
                        return (false, "Insufficient balance in the selected account.", null);
                }

                // 🔥 VALIDATION: for Income, ensure TO account exists
                if (transaction.Type == TransactionTypeEnum.Income)
                {
                    if (toAccount == null)
                        return (false, "Target account not found.", null);
                }

                // UPDATE BALANCES
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

                // SAVE 
                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveChangesAsync();
                _logger.LogInformation($"Transaction {transaction.Id} is created successfully");

                return (true, "Transaction created successfully.", transaction);
            }
            //catch (Exception ex) 
            //{
            //    _logger.LogError(ex, "Transaction of ");
            //}
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating transaction for UserId {UserId} ",
                    transaction.UserId
                );

                throw;
            }

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

            if (originalTx.IsReversed)
                return (false, "Transaction is already reversed.");

            // Mark original as reversed FIRST
            originalTx.IsReversed = true;

            await _transactionRepository.UpdateAsync(originalTx);

            // Reverse balance logic
            var oldFrom = originalTx.FromAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(originalTx.FromAccountId.Value)
                : null;

            var oldTo = originalTx.ToAccountId.HasValue
                ? await _accountRepository.GetByIdAsync(originalTx.ToAccountId.Value)
                : null;

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

            // Create reversal entry
            var reversalTx = new Transaction
            {
                UserId = originalTx.UserId,
                Amount = originalTx.Amount,
                Date = DateTime.Now,
                Note = $"REVERSAL of #{originalTx.Id}",
                Type = originalTx.Type,

                IsReversed = true,          // NEW FIX: reversal entries should also be non-reversible
                IsReversalEntry = true,

                FromAccountId = originalTx.ToAccountId,
                ToAccountId = originalTx.FromAccountId,
                ParentTransactionId = originalTx.Id
            };

            await _transactionRepository.AddAsync(reversalTx);

            // Audit log
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
                NewNote = reversalTx.Note,
                ChangedByUserId = originalTx.UserId
            };

            await _auditLogRepository.AddAsync(audit);

            // Save everything
            await _transactionRepository.SaveChangesAsync();
            await _auditLogRepository.SaveChangesAsync();

            return (true, "Transaction reversed successfully");
        }


        //    public async Task<(bool IsSuccess, string Message)> ReverseTransactionAsync(int id, string userId)
        //    {
        //        var originalTx = await GetTransactionAsync(id, userId);
        //        if (originalTx == null)
        //            return (false, "Transaction not found or unauthorized.");

        //        // Load accounts
        //        var oldFrom = originalTx.FromAccountId.HasValue
        //            ? await _accountRepository.GetByIdAsync(originalTx.FromAccountId.Value)
        //            : null;

        //        var oldTo = originalTx.ToAccountId.HasValue
        //            ? await _accountRepository.GetByIdAsync(originalTx.ToAccountId.Value)
        //            : null;
        //        originalTx.IsReversed = true;

        //        // 1. REVERSE ACCOUNT BALANCES
        //        switch (originalTx.Type)
        //        {
        //            case TransactionTypeEnum.Income:
        //                oldTo!.InitialBalance -= originalTx.Amount;
        //                break;

        //            case TransactionTypeEnum.Expense:
        //                oldFrom!.InitialBalance += originalTx.Amount;
        //                break;

        //            case TransactionTypeEnum.Transfer:
        //                oldTo!.InitialBalance -= originalTx.Amount;
        //                oldFrom!.InitialBalance += originalTx.Amount;
        //                break;
        //        }
        //      await _transactionRepository.UpdateAsync(originalTx);
        //        await _transactionRepository.SaveChangesAsync();

        //        // 2. CREATE REVERSAL TRANSACTION
        //        var reversalTx = new Transaction
        //        {
        //            UserId = originalTx.UserId,
        //            Amount = originalTx.Amount,
        //            Date = DateTime.Now,
        //            Note = $"REVERSAL of #{originalTx.Id}",
        //            Type = originalTx.Type,
        //            IsReversed = false,
        //            IsReversalEntry = true,
        //            ParentTransaction = originalTx,
        //            FromAccountId = originalTx.ToAccountId,
        //            ToAccountId = originalTx.FromAccountId
        //        };

        //        await _transactionRepository.AddAsync(reversalTx);

        //        // 3. CREATE AUDIT LOG ENTRY
        //        var audit = new TransactionAuditLog
        //        {
        //            TransactionId = originalTx.Id,
        //            AuditType = AuditTypeEnum.Reversed,

        //            OldAmount = originalTx.Amount,
        //            NewAmount = originalTx.Amount,

        //            OldFromAccountId = originalTx.FromAccountId,
        //            NewFromAccountId = reversalTx.FromAccountId,

        //            OldToAccountId = originalTx.ToAccountId,
        //            NewToAccountId = reversalTx.ToAccountId,

        //            OldDate = originalTx.Date,
        //            NewDate = reversalTx.Date,

        //            OldNote = originalTx.Note,
        //            NewNote = reversalTx.Note,

        //            ChangedByUserId = originalTx.UserId

        //        };

        //        await _auditLogRepository.AddAsync(audit);

        //        // COMMIT ALL CHANGES
        //        await _transactionRepository.SaveChangesAsync();
        //        await _auditLogRepository.SaveChangesAsync();

        //        return (true, "Transaction reversed successfully");
        //    }
    }
}
