using System.Security.Principal;
using EcommerceApi.DTOs;
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
        public DbSet<Shopping_cart> Shopping_carts { get; set; }
        public DbSet<Shopping_cart_item> Shopping_Cart_Items { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public object Seller { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Seller>().ToTable("sellers");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<Order_item>().ToTable("order_items");
            modelBuilder.Entity<Shopping_cart>().ToTable("shopping_cart");
            modelBuilder.Entity<Shopping_cart_item>().ToTable("shopping_cart_item");         
            modelBuilder.Entity<Token>().ToTable("tokens");

            modelBuilder.Entity<GetClientByEmialDto>().HasNoKey();
            modelBuilder.Entity<InsertedOrderDTO>().HasNoKey();
            modelBuilder.Entity<UserDTO>().HasNoKey();
            modelBuilder.Entity<TestDTO>().HasNoKey();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.order_items)
                .WithOne(oi => oi.order)
                .HasForeignKey(oi => oi.order_id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.client)
                .WithMany(c => c.orders)
                .HasForeignKey(o => o.client_id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.seller)
                .WithMany(s => s.orders)
                .HasForeignKey(o => o.seller_id);

            modelBuilder.Entity<Order_item>()
                .HasOne(i => i.order)
                .WithMany(o => o.order_items)
                .HasForeignKey(i => i.order_id);

            modelBuilder.Entity<Order_item>()
                .HasOne(i => i.product)
                .WithMany(p => p.order_Items)
                .HasForeignKey(i => i.product_id);
            
            modelBuilder.Entity<Shopping_cart>()
                .HasOne(s => s.client)
                .WithMany(c => c.shopping_Carts)
                .HasForeignKey(s => s.client_id);
            
            modelBuilder.Entity<Shopping_cart_item>()
                .HasOne(si => si.products)
                .WithMany(p => p.shopping_Cart_Items)
                .HasForeignKey(si => si.product_id);
            
            modelBuilder.Entity<Shopping_cart_item>()
                .HasOne(si => si.shopping_Carts)
                .WithMany(s => s.shopping_Cart_Items)
                .HasForeignKey(si => si.shopping_cart_id);

        }
    }


}