using DemoMinimalWebAPI.Context;
using DemoMinimalWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalWebAPI.API.Products
{
    internal static class ProductAPI
    {
        public static void AddProductAPI(this WebApplication app)
        {
            var productsApi = app.MapGroup("/products");
            productsApi.MapGet("/", async (MinimalContext db) =>
            {
                var products = await db.Products
                    .Include(p => p.Ingredients)
                    .AsNoTracking()
                    .ToListAsync();
                return products.Select(p => p.AsProductDto());
            });
            productsApi.MapGet("/{id}", async (MinimalContext db, [FromRoute]int id) =>
            {
                var product = await db.Products
                    .Include(p => p.Ingredients)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
                return product is not null ? Results.Ok(product.AsProductDto()) : Results.NotFound();
            });
            productsApi.MapPost("/", async (MinimalContext db, [FromBody]Product product) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Created($"/products/{product.Id}", product.AsProductDto());
            });
            productsApi.MapPut("/{id}", async (MinimalContext db, [FromRoute]int id, [FromBody] Product product) =>
            {
                if (id != product.Id)
                {
                    return Results.BadRequest("Product ID mismatch.");
                }
                var existingProduct = await db.Products.FindAsync(id);
                if (existingProduct is null)
                {
                    return Results.NotFound();
                }
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Ingredients = product.Ingredients;
                db.Products.Update(existingProduct);
                await db.SaveChangesAsync();
                return Results.Ok(existingProduct.AsProductDto());
            });
            productsApi.MapDelete("/{id}", async (MinimalContext db, [FromRoute]int id) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            productsApi.MapGet("/{id}/available", async (MinimalContext db, [FromRoute] int id) =>
            {
                var product = await db.Products
                    .Include(p => p.Ingredients)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                //as dto
                return Results.Ok(product.AsProductDto().IsAvailable);
            });
        }
    }
}
