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

        Task<Account?> GetByIdAsync(int id, string userId, CancellationToken ct = default);
    
        Task<List<Account>> ListByUserAsync(string userId, CancellationToken ct = default);

        Task<Account> AddAsync(Account account, string userId, CancellationToken ct = default);
        Task<Account?> UpdateAsync(Account account, string userId, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, string userId, CancellationToken ct = default);
    }
}
