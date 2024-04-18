using fw_shop_api.Models.Domain;

namespace fw_shop_api.Data.Interfaces
{
    public interface IImageRepository
    {
        Task<ProductImage> Upload(IFormFile file, ProductImage image);
        Task<IEnumerable<ProductImage>> GetAllImages();
    }
}