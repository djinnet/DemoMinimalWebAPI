namespace DemoMinimalWebAPI.Models
{
    internal class Ingredient
    {
        public int Id { get; set; }
        // optional link to Product id
        public int? ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsInStock { get; set; }

        // optional navigation property to Product
        public Product? Product { get; set; } = null;
    }

    internal class IngredientDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public bool IsInStock { get; set; }

        public ProductDto? Products { get; set; }
    }

    internal static class IngredientExtensions
    {
        public static IngredientDto AsIngredientDto(this Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                IsInStock = ingredient.IsInStock
            };
        }
    }
}
