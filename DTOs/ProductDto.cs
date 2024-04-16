namespace fw_shop_api.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public int Amount { get; set; } = 0;
        public bool IsAvailable { get; set; } = false;
        public string Url { get; set; } = string.Empty;
        public List<CategoryDto>? Categories { get; set; } = [];
    }
}