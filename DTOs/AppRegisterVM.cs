using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class AppRegisterVM
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}