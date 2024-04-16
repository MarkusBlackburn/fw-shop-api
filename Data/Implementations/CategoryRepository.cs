using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace fw_shop_api.Data.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByUrl(string url)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.UrlHandle == url);
        }

        public async Task<Category?> UpdateCategoryById(Category category)
        {
            var exCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (exCategory != null)
            {
                _dbContext.Entry(exCategory).CurrentValues.SetValues(category);
                await _dbContext.SaveChangesAsync();

                return category;
            }
            
            return null;
        }

        public async Task<Category?> DeleteCategoryById(Guid id)
        {
            var exCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (exCategory != null)
            {
                _dbContext.Categories.Remove(exCategory);
                await _dbContext.SaveChangesAsync();
                return exCategory;
            }

            return null;
        }
    }
}