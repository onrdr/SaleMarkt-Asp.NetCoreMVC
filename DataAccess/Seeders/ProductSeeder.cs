using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders;

public static class ProductSeeder
{
    public static ModelBuilder SeedProducts(this ModelBuilder modelBuilder)
    {
        var products = GetProducts();
        foreach (var product in products)
        {
            modelBuilder.Entity<Product>().HasData(product);
        }
        return modelBuilder;
    }

    public static List<Product> GetProducts()
    {
        var products = new List<Product>()
        {
            new()
            {
                Title = "Fortune of Time",
                Author = "Billy Spark",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SWD9999001",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = Guid.Parse("34245a4d-0baa-4c22-8245-02abb9063b11"),
                ImageUrl = ""
            },
            new()
            {
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "CAW777777701",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("34245a4d-0baa-4c22-8245-02abb9063b11"),
                ImageUrl = ""
            },
            new()
            {
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "RITO5555501",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = Guid.Parse("34245a4d-0baa-4c22-8245-02abb9063b11"),
                ImageUrl = ""
            },
            new()
            {
                Title = "Cotton Candy",
                Author = "Abby Muscles",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "WS3333333301",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"),
                ImageUrl = ""
            },
            new()
            {
                Title = "Rock in the Ocean",
                Author = "Ron Parker",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SOTJ1111111101",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"),
                ImageUrl = ""
            },
            new()
            {
                Title = "Leaves and Wonders",
                Author = "Laura Phantom",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "FOT000000001",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = Guid.Parse("db9b235f-a5d6-49dc-8e95-022f443f8582"),
                ImageUrl = ""
            }
        };

        return products;
    }
}
