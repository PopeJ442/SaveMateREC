using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;


namespace Savemate.Infrastructure.Repository
{
    public class BaseRepository<T>(SaveMateDbContext context) : IBaseRepository<T> where T : class
    {

        protected readonly SaveMateDbContext _context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();



        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task AddAsync(T entity)
        {
            _dbSet.AddAsync(entity);
        }


        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);

        }


        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);

        }

        public async Task SaveChangesAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }

        
    }


}
