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
                Id = Guid.Parse("552E5AF1-2C2F-494C-AE4E-D3DEF3307D1A"),
                Title = "Fortune of Time",
                Author = "Billy Spark",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SWD9999001",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = ""
            },
            new()
            {
                Id = Guid.Parse("90CB6958-D27E-47DA-AAD9-67AB6CAD65C6"),
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "CAW777777701",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = ""
            },
            new()
            {   Id = Guid.Parse("A86E8DCE-41F7-4440-BA4F-4B6424B8DA00"),
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "RITO5555501",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = ""
            },
            new()
            {
                Id = Guid.Parse("32AED196-A986-40C5-8BAD-5E773F97E357"),
                Title = "Cotton Candy",
                Author = "Abby Muscles",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "WS3333333301",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = ""
            },
            new()
            {   
                Id = Guid.Parse("7CC4275F-0CE0-45CF-8359-E8B4CABAB400"),
                Title = "Rock in the Ocean",
                Author = "Ron Parker",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SOTJ1111111101",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = ""
            },
            new()
            {
                Id = Guid.Parse("66BD5F42-3718-47D6-90FD-16D69AEE93B1"),
                Title = "Leaves and Wonders",
                Author = "Laura Phantom",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "FOT000000001",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = ""
            }
        };

        return products;
    }
}
