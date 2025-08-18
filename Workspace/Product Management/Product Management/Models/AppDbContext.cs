using Microsoft.EntityFrameworkCore;

namespace Product_Management.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // - add-migration <name>
            // - remove-migration
            // - update-database
            // - C-R-U-D
        }
    }
}
