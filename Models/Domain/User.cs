using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fw_shop_api.Models.Domain
{
    [Table(nameof(User))]
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;

        [NotMapped]
        public string FullName
        {
            get{ return $"{LastName} {FirstName}";}
        }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public class Role : IdentityRole<long> {}
    public class UserClaim : IdentityUserClaim<long> {}
    public class UserRole : IdentityUserRole<long> {}
    public class UserLogin : IdentityUserLogin<long> {}
    public class RoleClaim : IdentityRoleClaim<long> {}
    public class UserToken : IdentityUserToken<long> {}
}