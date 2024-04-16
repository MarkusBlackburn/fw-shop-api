using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateNewProduct(Product product);
    }
}