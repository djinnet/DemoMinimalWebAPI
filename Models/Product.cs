namespace DemoMinimalWebAPI.Models
{
    internal class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = [];
    }

    internal class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable => Ingredients.Any(i => i.IsInStock);

        public List<IngredientDto> Ingredients { get; set; } = [];
    }

    internal static class ProductExtensions
    {
        public static ProductDto AsProductDto(this Product product)
        {
            var ingredients = product.Ingredients.Select(i => i.AsIngredientDto()).ToList();
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Ingredients = ingredients
            };
        }
    }
}
