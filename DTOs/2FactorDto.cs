using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class TwoFactorDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Provider { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}