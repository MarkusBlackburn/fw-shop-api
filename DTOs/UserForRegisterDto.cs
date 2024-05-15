using System.ComponentModel.DataAnnotations;

namespace fw_shop_api.DTOs
{
    public class UserForRegisterDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and confirmation password doesn't match")]
        public string? ConfirmPassword { get; set; }
        public string? ClientURI { get; set; }
    }
}