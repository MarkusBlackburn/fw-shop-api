using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace fw_shop_api.Data.Implementations
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContext, ApplicationDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContext;
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ProductImage>> GetAllImages()
        {
            return await _dbContext.ProductImages.ToListAsync();
        }

        public async Task<ProductImage> Upload(IFormFile file, ProductImage image)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "images", $"{image.Filename}{image.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);
            var httpRequest = _httpContext.HttpContext!.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/images/{image.Filename}{image.FileExtension}";
            image.ImageUrlPath = urlPath;

            await _dbContext.ProductImages.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}