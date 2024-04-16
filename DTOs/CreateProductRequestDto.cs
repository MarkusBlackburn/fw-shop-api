using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class CreateProductRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; } = decimal.Zero;
        public int Amount { get; set; } = 0;
        public bool IsAvailable { get; set; } = false;
        [Required]
        public string Url { get; set; } = string.Empty;
        public string[]? Categories { get; set; }
    }
}