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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SaveMateDbContext _context;

        public CategoryRepository(SaveMateDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
