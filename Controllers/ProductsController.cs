using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productRepository.GetAllProducts(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            var response = new List<ProductDto>();
            foreach (var product in products)
            {
                response.Add(new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    ShortDescription = product.ShortDescription,
                    Content = product.Content,
                    Price = product.Price,
                    Amount = product.Amount,
                    IsAvailable = product.IsAvailable,
                    Url = product.UrlHandle,
                    Categories = product.Categories!.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Url = c.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null) return NotFound("Product doesn't exist");

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
                Categories = product.Categories?.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Url = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("byUrl/{url}")]
        public async Task<IActionResult> GetProductByUrl(string url)
        {
            var product = await _productRepository.GetProductByUrl(url);
            if (product is null) return NotFound("Product doesn't exist");

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
                Categories = product.Categories?.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Url = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("ProductsByCategory/{id}")]
        public async Task<IActionResult> GetProductsByCategoryId(Guid id)
        {
            var category = await _productRepository.GetProductsByCategoryId(id);
            if (category is null) return NotFound("Category doesn't exist");

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.UrlHandle,
                Products = category.Products?.Select(c => new ProductDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortDescription = c.ShortDescription,
                    Content = c.Content,
                    Price = c.Price,
                    Amount = c.Amount,
                    IsAvailable = c.IsAvailable,
                    Url = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateProductById([FromRoute] int id,UpdateProductRequestDto request)
        {
            var product = new Product
            {
                Id = id,
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                Price = request.Price,
                Amount = request.Amount,
                IsAvailable = request.IsAvailable,
                UrlHandle = request.Url,
                Categories = []
            };

            foreach(var categoryUrl in request.Categories)
            {
                var exCategory = await _categoryRepository.GetCategoryByUrl(categoryUrl);
                if (exCategory is not null) product.Categories.Add(exCategory);
            }

            var updatedProduct = await _productRepository.UpdateProductById(product);
            if (updatedProduct is null) return NotFound ("Product doesn't exist");

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

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteProductById([FromRoute] int id)
        {
            var product = await _productRepository.DeleteProductById(id);
            if (product is null) return NotFound("Product doesn't exist");

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Content = product.Content,
                Price = product.Price,
                Amount = product.Amount,
                IsAvailable = product.IsAvailable,
                Url = product.UrlHandle
            };

            return Ok(response);
        }
    }
}