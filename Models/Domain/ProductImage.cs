namespace fw_shop_api.Models.Domain
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ImageUrlPath { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}