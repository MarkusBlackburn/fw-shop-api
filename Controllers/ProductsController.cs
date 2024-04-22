using System.Linq.Expressions;
using System.Text.Json;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var products = await _productRepository.GetAllProducts(filterOn, filterQuery, sortBy, isAscending ?? true);

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

        /*[HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] SearchParams searchParam)
        {
            List<ColumnFilter> columnFilters = [];
            if (!String.IsNullOrEmpty(searchParam.ColumnFilters))
            {
                try
                {
                    columnFilters.AddRange(JsonSerializer.Deserialize<List<ColumnFilter>>(searchParam.ColumnFilters));
                }
                catch (Exception) {columnFilters = [];}
            }

            List<ColumnSorting> columnSorting = [];
            if (!String.IsNullOrEmpty(searchParam.OrderBy))
            {
                try
                {
                    columnSorting.AddRange(JsonSerializer.Deserialize<List<ColumnSorting>>(searchParam.OrderBy));
                }
                catch (Exception) {columnSorting = [];}
            }

            Expression<Func<Product, bool>> filters = null;
            var searchTerm = "";
            if (!string.IsNullOrEmpty(searchParam.SearchTerm))
            {
                searchTerm = searchParam.SearchTerm.Trim().ToLower();
                filters = x => x.Name.ToLower().Contains(searchTerm);
            }

            if (columnFilters.Count > 0)
            {
                var customFilter = CustomExpressionFilter<Product>.CustomFilter(columnFilters, "products");
                filters = customFilter;
            }

            var query = products.AsQueryable().CustomQuery(filters);
            var count = query.Count();
            var filteredData = query.CustomPagination(searchParam.PageNumber, searchParam.PageSize).ToListAsync();
            var pagedList = new PagedList<Product>(filteredData, count, searchParam.PageNumber, searchParam.PageSize);

            if (pagedList is not null) Response.AddPaginationHeader(pagedList.MetaData);

            return Ok(pagedList);
        }*/
    }
}