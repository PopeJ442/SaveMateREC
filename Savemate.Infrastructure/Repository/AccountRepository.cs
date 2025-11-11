using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using Savemate.Infrastructure.Repository;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    private readonly SaveMateDbContext _context;

    public AccountRepository(SaveMateDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Account> AddAccountAsync(Account account, string userId, CancellationToken ct = default)
    {
        account.UserId = userId;
        await _context.Set<Account>().AddAsync(account, ct);
        await _context.SaveChangesAsync(ct);
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(int id, string userId, CancellationToken ct = default)
        => await _context.Set<Account>()
                         .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);

    public async Task<List<Account>> ListAccountByUserAsync(string userId, CancellationToken ct = default)
        => await _context.Set<Account>()
                         .Where(a => a.UserId == userId)
                         .ToListAsync(ct);

    public async Task<Account?> UpdateAccountAsync(Account account, string userId, CancellationToken ct = default)
    {
        var existing = await GetAccountByIdAsync(account.Id, userId, ct);
        if (existing == null) return null;

        existing.Name = account.Name;
        existing.Type = account.Type;
        existing.InitialBalance = account.InitialBalance;

        _context.Set<Account>().Update(existing);
        await _context.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteAccountAsync(int id, string userId, CancellationToken ct = default)
    {
        var account = await GetAccountByIdAsync(id, userId, ct);
        if (account == null) return false;

        _context.Set<Account>().Remove(account);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}

