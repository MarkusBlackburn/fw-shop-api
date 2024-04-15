using fw_shop_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace fw_shop_api.Data.App
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}