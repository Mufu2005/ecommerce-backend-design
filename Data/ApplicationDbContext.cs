using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using ShopHub.Models;

namespace ShopHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        // This maps your "Products" to a MongoDB Collection
        public DbSet<ProductViewModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the "Product" entity to a collection named "products"
            modelBuilder.Entity<ProductViewModel>().ToCollection("products");
        }
    }
}
