using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Interface.IRepositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {

        Task<Account?> GetAccountByIdAsync(int id, string userId, CancellationToken ct = default);
    
        Task<List<Account>> ListAccountByUserAsync(string userId, CancellationToken ct = default);

        Task<Account> AddAccountAsync(Account account, string userId, CancellationToken ct = default);
        Task<Account?> UpdateAccountAsync(Account account, string userId, CancellationToken ct = default);
        Task<bool> DeleteAccountAsync(int id, string userId, CancellationToken ct = default);
    }
}
