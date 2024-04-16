namespace fw_shop_api.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UrlHandle { get; set; } = string.Empty;
        public ICollection<Product>? Products { get; set; }
    }
}