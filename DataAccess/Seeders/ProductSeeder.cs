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
                Id = Guid.NewGuid(),
                Title = "Product 1",
                Color = "Red",
                Size = "M",
                Description = "Product 1 Description",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 2",
                Color = "Blue",
                Size = "S", 
                Description = "Product 2 Description",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 3",
                Color = "Green",
                Size = "L",
                Description = "Product 3 Description",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\product.png"
            },
           new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 4",
                Color = "Black",
                Size = "XL",
                Description = "Product 4 Description",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 5",
                Color = "White",
                Size = "M",
                Description = "Product 5 Description",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 6",
                Color = "Yellow",
                Size = "L",
                Description = "Product 6 Description",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 7",
                Color = "Red",
                Size = "S",
                Description = "Product 7 Description",
                ListPrice = 60,
                Price = 55,
                Price50 = 50,
                Price100 = 45,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 8",
                Color = "Blue",
                Size = "M",
                Description = "Product 8 Description",
                ListPrice = 45,
                Price = 40,
                Price50 = 38,
                Price100 = 35,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 9",
                Color = "Green",
                Size = "L",
                Description = "Product 9 Description",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 10",
                Color = "Black",
                Size = "XL",
                Description = "Product 10 Description",
                ListPrice = 35,
                Price = 32,
                Price50 = 30,
                Price100 = 28,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 11",
                Color = "White",
                Size = "S",
                Description = "Product 11 Description",
                ListPrice = 25,
                Price = 23,
                Price50 = 21,
                Price100 = 19,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 12",
                Color = "Yellow",
                Size = "M",
                Description = "Product 12 Description",
                ListPrice = 50,
                Price = 45,
                Price50 = 42,
                Price100 = 40,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 13",
                Color = "Red",
                Size = "L",
                Description = "Product 13 Description",
                ListPrice = 40,
                Price = 38,
                Price50 = 35,
                Price100 = 32,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 14",
                Color = "Blue",
                Size = "XL",
                Description = "Product 14 Description",
                ListPrice = 55,
                Price = 52,
                Price50 = 50,
                Price100 = 48,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 15",
                Color = "Green",
                Size = "S",
                Description = "Product 15 Description",
                ListPrice = 70,
                Price = 68,
                Price50 = 65,
                Price100 = 60,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\product.png"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 16",
                Color = "Black",
                Size = "M",
                Description = "Product 16 Description",
                ListPrice = 45,
                Price = 42,
                Price50 = 40,
                Price100 = 38,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\product.png"
            }
        };

        return products;
    }
}
