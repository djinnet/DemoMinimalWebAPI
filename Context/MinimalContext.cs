using DemoMinimalWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DemoMinimalWebAPI.Context
{
    internal class MinimalContext : DbContext
    {
        public MinimalContext(DbContextOptions<MinimalContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set the name for the database
            modelBuilder.HasDefaultSchema("DemoMinimalWebAPI");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Ingredients)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //Seed data with products and ingredients. For example, we can add some initial products and ingredients. Such as pizza and its ingredients.
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Pizza", Price = 9.99m },
                new Product { Id = 2, Name = "Burger", Price = 5.99m }
            );
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Cheese", IsInStock = true, ProductId = 1 },
                new Ingredient { Id = 2, Name = "Tomato Sauce", IsInStock = true, ProductId = 1 },
                new Ingredient { Id = 3, Name = "Lettuce", IsInStock = false, ProductId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
    }
}
