using DemoMinimalWebAPI.API.Ingredients;
using DemoMinimalWebAPI.API.Products;
using DemoMinimalWebAPI.Context;
using DemoMinimalWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DemoMinimalWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);
            builder.Services.AddDbContext<MinimalContext>(options =>
            {
                //options.UseInMemoryDatabase("DemoMinimalWebAPI");
                //get connection string from appsettings.json
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
            });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }
            

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            });

            var app = builder.Build();

            app.AddProductAPI();
            app.AddIngredientsAPI();

            app.Run();
        }
    }

    [JsonSerializable(typeof(Product[]))]
    [JsonSerializable(typeof(Ingredient[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}
