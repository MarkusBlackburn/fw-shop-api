using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class JwtResponseVM
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}