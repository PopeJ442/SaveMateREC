using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Application.Services.IService.IAccountService;
using Savemate.Domain.Entities;

namespace Savemate.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> AddAccount(Account account, string userId)
        {
            await _accountRepository.AddAccountAsync(account, userId);
            return account;
        }

        public async Task<bool> DeleteAccount(int id, string userId)
        {
            return await _accountRepository.DeleteAccountAsync(id, userId);
        }

        public async Task<Account?> UpdateAccount(Account account, string userId)
        {
            return await _accountRepository.UpdateAccountAsync(account, userId);
        }

        public async Task<Account?> GetAccountById(int accountId, string userId)
        {
            return await _accountRepository.GetAccountByIdAsync(accountId, userId);
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserAsync(string userId)
        {
            return await _accountRepository.ListAccountByUserAsync(userId);
        }
    }

}
