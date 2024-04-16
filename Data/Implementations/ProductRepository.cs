using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Product> CreateNewProduct(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }
    }
}