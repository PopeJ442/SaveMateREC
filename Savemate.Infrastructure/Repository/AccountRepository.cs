using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure.Repository
{
    public class AccountRepository(SaveMateDbContext context) : BaseRepository<Account>(context), IAccountRepository
    {
        public async Task<Account?> GetByIdAsync(int id, string userId, CancellationToken ct = default)
    => await _context.Set<Account>()
        .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);


        public async Task<List<Account>> ListByUserAsync(string userId, CancellationToken ct = default)
            => await _context.Set<Account>()
                             .Where(a => a.UserId == userId)
                             .ToListAsync(ct);

        public async Task<Account> AddAsync(Account account, string userId, CancellationToken ct = default)
        {
            account.UserId = userId; // enforce ownership
            await _context.Set<Account>().AddAsync(account, ct);
            await _context.SaveChangesAsync(ct);
            return account;
        }

        public async Task<Account?> UpdateAsync(Account account, string userId, CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(account.Id, userId, ct);
            if (existing == null) return null;

            // Update fields
            existing.Name = account.Name;
            existing.Type = account.Type;
            existing.InitialBalance = account.InitialBalance;

            _context.Set<Account>().Update(existing);
            await _context.SaveChangesAsync(ct);

            return existing;
        }

        public async Task<bool> DeleteAsync(int id, string userId, CancellationToken ct = default)
        {
            var account = await GetByIdAsync(id, userId, ct);
            if (account == null) return false;

            _context.Set<Account>().Remove(account);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }

}
 