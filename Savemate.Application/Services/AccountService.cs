using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;

namespace Savemate.Application.Services
{
    public class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<Account> AddAccount(Account account)
        {
            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();
            return account;
        }

        public async Task DeleteAccount(Account account)
        {
            await _accountRepository.DeleteAsync(account);
            await _accountRepository.SaveChangesAsync();
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            await _accountRepository.UpdateAsync(account);
            await _accountRepository.SaveChangesAsync();
            return account;
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<IEnumerable<Account>> GetAllAccount()
        {
            return await _accountRepository.GetAllAsync();
        }

        public Task<IEnumerable<object>> GetAccountsByUserAsync(string? userId)
        {
            throw new NotImplementedException();
        }
    }
}
