using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}