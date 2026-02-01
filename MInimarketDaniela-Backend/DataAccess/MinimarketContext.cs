using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.DataAccess
{
    public class MinimarketContext : DbContext
    {
        public MinimarketContext(DbContextOptions<MinimarketContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => p.Barcode).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}
