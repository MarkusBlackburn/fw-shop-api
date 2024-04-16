using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(CreateProductRequestDto request)
        {
            var product = new Product
            {
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                Price = request.Price,
                Amount = request.Amount,
                IsAvailable = request.IsAvailable,
                UrlHandle = request.Url,
                Categories = []
            };

            foreach (var categoryUrl in request.Categories!)
            {
                var exCategory = await _categoryRepository.GetCategoryByUrl(categoryUrl);

                if (exCategory is not null) product.Categories.Add(exCategory);
            }

            product = await _productRepository.CreateNewProduct(product);

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Content = product.Content,
                Price = product.Price,
                Amount = product.Amount,
                IsAvailable = product.IsAvailable,
                Url = product.UrlHandle,
                Categories = product.Categories!.Select(c => new CategoryDto{
                    Id = c.Id,
                    Name = c.Name,
                    Url = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }
    }
}