using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class CreateCategoryRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
    }
}