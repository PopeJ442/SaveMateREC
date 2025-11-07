using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services.IService
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryAsync(int id, string userId);
        Task<IEnumerable<Category>> GetCategoriesByUserAsync(string userId);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id, string userId);
    }
}
