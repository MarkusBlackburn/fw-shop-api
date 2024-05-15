using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class UserForAuthDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ClientURI { get; set; }
    }
}