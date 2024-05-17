using fw_shop_api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace fw_shop_api.Data.App
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRole = "9fe09393-2fad-48fd-b5b2-b1335adc5599";
            var writerRole = "27b9fe2e-e8bd-458a-9dcf-379993956ae3";

            var roles = new List<IdentityRole>
            {
                new()
                {
                    Id = readerRole,
                    Name = "Viewer",
                    NormalizedName = "Viewer".ToUpper(),
                },

                new()
                {
                    Id = writerRole,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            
            var adminId = "ec4da958-8c96-4d74-8eb5-32a64facf86d";
            var admin = new IdentityUser()
            {
                Id = adminId,
                UserName = "Admin",
                Email = "gudvinrawson@gmail.com",
                NormalizedEmail = "gudvinrawson@gmail.com".ToUpper(),
                NormalizedUserName = "Admin".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "hD8g!1$edn");
            builder.Entity<IdentityUser>().HasData(admin);

            /*var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminId,
                    RoleId = writerRole
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);*/
        }
    }
}