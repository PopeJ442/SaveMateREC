using Microsoft.EntityFrameworkCore;
using Savemate.Application.Interface.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure.Repository
{
    public class BaseRepository<T>(SaveMateDbContext context) : IBaseRepository<T> where T : class
    {
         
        protected readonly SaveMateDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        

        public async Task<T> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity)
        {
            _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
           

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(T entity) 
        {
           _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }
    }


}
