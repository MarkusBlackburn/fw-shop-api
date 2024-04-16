using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class GoogleSignInVM
    {
        [Required]
        public string? IdToken { get; set; }
    }
}