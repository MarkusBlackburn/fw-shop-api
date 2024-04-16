using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryByUrl(string url);
        Task<Category?> UpdateCategoryById(Category category);
        Task<Category?> DeleteCategoryById(Guid id);
    }
}