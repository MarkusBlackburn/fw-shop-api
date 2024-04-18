using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAllImages();
            var response = new List<ProductImageDto>();
            foreach (var image in images)
            {
                response.Add(new ProductImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    Filename = image.Filename,
                    FileExtension = image.FileExtension,
                    ImageUrlPath = image.ImageUrlPath,
                    DateCreated = image.DateCreated
                });
            }

            return Ok(response);    
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                var productImage = new ProductImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Filename = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                productImage = await _imageRepository.Upload(file, productImage);

                var response = new ProductImageDto
                {
                    Id = productImage.Id,
                    Title = productImage.Title,
                    DateCreated = productImage.DateCreated,
                    FileExtension = productImage.FileExtension,
                    Filename = productImage.Filename,
                    ImageUrlPath = productImage.ImageUrlPath
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] {".jpg", ".jpeg", ".png"};

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size can't be more 10MB");
            }
        }
    }
}