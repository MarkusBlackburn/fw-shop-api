using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.Url
            };

            if (ModelState.IsValid)
            {
                await _repository.CreateCategory(category);

                var response = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Url = category.UrlHandle
                };

                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _repository.GetAllCategories();

            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Url = category.UrlHandle
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{url}")]
        public async Task<IActionResult> GetCategoryByUrl([FromRoute] string url)
        {
            var category = await _repository.GetCategoryByUrl(url);

            if (category == null) return NotFound("Category doesn't exist");

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.UrlHandle
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCategoryById([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.Url
            };

            category = await _repository.UpdateCategoryById(category);

            if (category is null) return NotFound("Category doesn't exist");

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.UrlHandle
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] Guid id)
        {
            var category = await _repository.DeleteCategoryById(id);

            if (category is null) return NotFound("Category doesn't exist");

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.UrlHandle
            };

            return Ok(response);
        }
    }
}