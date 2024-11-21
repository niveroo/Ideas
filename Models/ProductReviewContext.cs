using Microsoft.EntityFrameworkCore;

namespace Ideas.Models
{
    public class ProductReviewContext : DbContext
    {
        public ProductReviewContext(DbContextOptions<ProductReviewContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
    }
}