using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products.Include(c => c.Categories).ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _dbContext.Products.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product?> GetProductByUrl(string url)
        {
            return await _dbContext.Products.Include(c => c.Categories).FirstOrDefaultAsync(c => c.UrlHandle == url);
        }

        public async Task<Category?> GetProductsByCategoryId(Guid id)
        {
            return await _dbContext.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product?> UpdateProductById(Product product)
        {
            var exProduct = await _dbContext.Products.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == product.Id);
            if (exProduct is null) return null;

            _dbContext.Entry(exProduct).CurrentValues.SetValues(product);
            exProduct.Categories = product.Categories;

            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> DeleteProductById(int id)
        {
            var exProduct = await _dbContext.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (exProduct is null) return null;

            _dbContext.Products.Remove(exProduct);
            await _dbContext.SaveChangesAsync();

            return exProduct;
        }
    }
}