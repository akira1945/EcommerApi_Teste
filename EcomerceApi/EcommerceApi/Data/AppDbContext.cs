using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_item> Order_Items { get; set; }
        public object Seller { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("clients");

            modelBuilder.Entity<Seller>().ToTable("sellers");

            modelBuilder.Entity<Product>().ToTable("products");

            modelBuilder.Entity<Order>().ToTable("orders");

            modelBuilder.Entity<Order_item>().ToTable("order_items");
        }
    }

                 
}