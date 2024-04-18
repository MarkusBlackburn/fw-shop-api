namespace fw_shop_api.DTOs
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ImageUrlPath { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}