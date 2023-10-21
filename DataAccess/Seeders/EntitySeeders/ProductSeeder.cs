using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

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
                Description = "Product 1 Description",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = "images\\product\\2153c139-8a7b-415c-bbd5-fa85ca4144ae.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 2",
                Color = "Blue", 
                Description = "Product 2 Description",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\aba380dd-547a-4d03-9af1-ff51280affd4.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 3",
                Color = "Green", 
                Description = "Product 3 Description",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\f611fc13-641c-4bec-a38e-6a4e32c985a3.jpg"
            },
           new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 4",
                Color = "Black", 
                Description = "Product 4 Description",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\d797b2fa-9264-49cb-85d7-c0c8a67cba39.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 5",
                Color = "White", 
                Description = "Product 5 Description",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\d0af45f8-c81f-4dd3-8fdc-b3edc342feeb.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 6",
                Color = "Yellow", 
                Description = "Product 6 Description",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\ec7c0a2b-f42c-40f0-975d-7c15c44086df.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 7",
                Color = "Red", 
                Description = "Product 7 Description",
                ListPrice = 60,
                Price = 55,
                Price50 = 50,
                Price100 = 45,
                CategoryId = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                ImageUrl = "images\\product\\7f8cc983-30c3-42d3-bb5e-f36c71893b42.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 8",
                Color = "Blue", 
                Description = "Product 8 Description",
                ListPrice = 45,
                Price = 40,
                Price50 = 38,
                Price100 = 35,
                CategoryId = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                ImageUrl = "images\\product\\e7cda191-52f9-4afe-82f2-c57108f2f637.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 9",
                Color = "Green", 
                Description = "Product 9 Description",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\df175402-f3ae-4fe5-a6f1-a779be9aff3a.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 10",
                Color = "Black", 
                Description = "Product 10 Description",
                ListPrice = 35,
                Price = 32,
                Price50 = 30,
                Price100 = 28,
                CategoryId = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                ImageUrl = "images\\product\\368c250f-7afb-4e8f-9749-daacdb784cd8.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 11",
                Color = "White", 
                Description = "Product 11 Description",
                ListPrice = 25,
                Price = 23,
                Price50 = 21,
                Price100 = 19,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\a67ee540-cf74-424b-bb51-acca616cd9d1.jpg"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product 12",
                Color = "Yellow", 
                Description = "Product 12 Description",
                ListPrice = 50,
                Price = 45,
                Price50 = 42,
                Price100 = 40,
                CategoryId = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                ImageUrl = "images\\product\\b0ea900b-6fef-4c80-8ba0-03669b5576b1.jpg"
            }
        };

        return products;
    }
}
