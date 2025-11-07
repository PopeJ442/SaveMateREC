using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services.IService
{
    public interface IAccountService 
    {
        Task<Account> AddAccount(Account account);
        Task DeleteAccount(Account account);
        Task<Account> UpdateAccount(Account account);
        Task<Account> GetAccountById(int accountId);
        Task<IEnumerable<Account>> GetAllAccount ();
        Task<IEnumerable<object>> GetAccountsByUserAsync(string? userId);
    }
}
