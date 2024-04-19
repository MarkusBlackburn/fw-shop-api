using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateNewProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task<Product?> GetProductByUrl(string url);
        Task<Category?> GetProductsByCategoryId(Guid id);
    }
}