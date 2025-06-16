using DemoMinimalWebAPI.Context;
using DemoMinimalWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DemoMinimalWebAPI.API.Ingredients
{
    internal static class IngredientsAPI
    {
        public static void AddIngredientsAPI(this WebApplication app)
        {
            var ingredientsApi = app.MapGroup("/ingredients");
            ingredientsApi.MapGet("/", async (MinimalContext db) =>
            {
                var ingredients = await db.Ingredients.AsNoTracking().ToListAsync();
                return Results.Ok(ingredients);
            });
            ingredientsApi.MapGet("/{id}", async (MinimalContext db, [FromRoute] int id) =>
            {
                var ingredient = await db.Ingredients.FindAsync(id);
                return ingredient is not null ? Results.Ok(ingredient) : Results.NotFound();
            });
            ingredientsApi.MapPost("/", async (MinimalContext db, [FromBody]Ingredient ingredient) =>
            {
                db.Ingredients.Add(ingredient);
                await db.SaveChangesAsync();
                return Results.Created($"/ingredients/{ingredient.Id}", ingredient);
            });
            ingredientsApi.MapPut("/{id}", async (MinimalContext db, [FromRoute]int id, [FromBody]Ingredient ingredient) =>
            {
                if (id != ingredient.Id)
                {
                    return Results.BadRequest("Ingredient ID mismatch.");
                }
                var existingIngredient = await db.Ingredients.FindAsync(id);
                if (existingIngredient is null)
                {
                    return Results.NotFound();
                }
                existingIngredient.Name = ingredient.Name;
                existingIngredient.IsInStock = ingredient.IsInStock;
                db.Ingredients.Update(existingIngredient);
                await db.SaveChangesAsync();
                return Results.Ok(existingIngredient);
            });
            ingredientsApi.MapDelete("/{id}", async (MinimalContext db, [FromRoute] int id) =>
            {
                var ingredient = await db.Ingredients.FindAsync(id);
                if (ingredient is null)
                {
                    return Results.NotFound();
                }
                db.Ingredients.Remove(ingredient);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
