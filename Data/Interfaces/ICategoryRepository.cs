using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategory(Category category);
    }
}