using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> GetCategoryAsync(int id, string userId)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return (category != null && category.UserId == userId) ? category : null;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserAsync(string userId)
        {
            return await _categoryRepository.GetByUserIdAsync(userId);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            var category = await GetCategoryAsync(id, userId);
            if (category == null)
                throw new InvalidOperationException("Category not found or unauthorized.");

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

    }
}
