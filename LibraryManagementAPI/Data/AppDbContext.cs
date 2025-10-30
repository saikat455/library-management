using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Sale>()
                .HasKey(s => s.SaleId);

            // Configure relationships
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);

            // Seed data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 101, ProductName = "Laptop", Category = "Electronics", UnitPrice = 500.00M },
                new Product { ProductId = 102, ProductName = "Smartphone", Category = "Electronics", UnitPrice = 300.00M },
                new Product { ProductId = 103, ProductName = "Headphones", Category = "Electronics", UnitPrice = 30.00M },
                new Product { ProductId = 104, ProductName = "Keyboard", Category = "Electronics", UnitPrice = 20.00M },
                new Product { ProductId = 105, ProductName = "Mouse", Category = "Electronics", UnitPrice = 15.00M }
            );

            modelBuilder.Entity<Sale>().HasData(
                new Sale { SaleId = 1, ProductId = 101, QuantitySold = 5, SaleDate = new DateTime(2024, 1, 1), TotalPrice = 2500.00M },
                new Sale { SaleId = 2, ProductId = 102, QuantitySold = 3, SaleDate = new DateTime(2024, 1, 2), TotalPrice = 900.00M },
                new Sale { SaleId = 3, ProductId = 103, QuantitySold = 2, SaleDate = new DateTime(2024, 1, 2), TotalPrice = 60.00M },
                new Sale { SaleId = 4, ProductId = 104, QuantitySold = 4, SaleDate = new DateTime(2024, 1, 3), TotalPrice = 80.00M },
                new Sale { SaleId = 5, ProductId = 105, QuantitySold = 6, SaleDate = new DateTime(2024, 1, 3), TotalPrice = 90.00M }
            );
        }
    }
}