using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateNewProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true);
        Task<Product?> GetProductById(int id);
        Task<Product?> GetProductByUrl(string url);
        Task<Category?> GetProductsByCategoryId(Guid id);
        Task<Product?> UpdateProductById(Product product);
        Task<Product?> DeleteProductById(int id);
    }
}