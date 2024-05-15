using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and confirmation password doesn't match")]
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}