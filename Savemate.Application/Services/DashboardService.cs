using Microsoft.AspNetCore.Identity;
using Savemate.Application.Interface;
using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Interfaces.Repositories;
using Savemate.Application.Services.IService;
using Savemate.Application.ViewModels;
using Savemate.Domain.Enums;
using Savemate.Infrastructure;

namespace Savemate.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardService(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            UserManager<ApplicationUser> userManager)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _userManager = userManager;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync(string userId)
        {
         
             var accounts = await _accountRepository.ListAccountByUserAsync(userId);
             var transactions = await _transactionRepository.GetByUserIdAsync(userId);

             var totalBalance = accounts.Sum(a =>a.InitialBalance);
             
             var totalIncome =  transactions.Where(t => t.Type ==  TransactionTypeEnum.Income && !t.IsReversed).Sum(t =>t.Amount);
            var totalExpense = transactions
        .Where(t => t.Type == TransactionTypeEnum.Expense && !t.IsReversed)
        .Sum(t => t.Amount);

            var recentTransaction = transactions.OrderByDescending(t => t.Date).Take(5).Select(t => new TransactionDTO {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                Date = t.Date,
                FromAccountName = t.FromAccount?.Name,
                ToAccountName = t.ToAccount?.Name
            });
            return new DashboardDTO
            {
                TotalBalance = totalBalance,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                RecentTransactions = recentTransaction.ToList(),
            };
        }
         
    }
}
