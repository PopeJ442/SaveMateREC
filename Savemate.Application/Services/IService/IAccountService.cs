using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services.IService.IAccountService
{
    public interface IAccountService 
    {
        
            Task<Account> AddAccount(Account account, string userId);
            Task<bool> DeleteAccount(int id, string userId);
            Task<Account?> UpdateAccount(Account account, string userId);
            Task<Account?> GetAccountById(int accountId, string userId);
            Task<IEnumerable<Account>> GetAllAccounts();
            Task<IEnumerable<Account>> GetAccountsByUserAsync(string userId);
        

    }
}
