using Microsoft.AspNetCore.Identity;

namespace fw_shop_api.Models.Domain
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}