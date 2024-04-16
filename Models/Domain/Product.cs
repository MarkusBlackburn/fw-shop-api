using System.ComponentModel.DataAnnotations.Schema;

namespace fw_shop_api.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = decimal.Zero;
        public int Amount { get; set; } = 0;
        public bool IsAvailable { get; set; } = false;
        public string UrlHandle { get; set; } = string.Empty;
        public ICollection<Category>? Categories { get; set; }
    }
}