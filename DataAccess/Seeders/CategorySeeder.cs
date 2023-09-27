using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders;

public static class CategorySeeder
{
    public static ModelBuilder SeedCategories(this ModelBuilder modelBuilder)
    {
        var categories = GetCategories();
        foreach (var category in categories)
        {
            modelBuilder.Entity<Category>().HasData(category);
        }
        return modelBuilder;
    }

    public static List<Category> GetCategories()
    {
        var categories = new List<Category>()
        {
            new Category()
            {
                Id = Guid.Parse("34245a4d-0baa-4c22-8245-02abb9063b11"),
                Name = "Action",
                DisplayOrder = 1
            },
            new Category()
            {
                Id = Guid.Parse("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"),
                Name = "SciFi",
                DisplayOrder = 2
            },
            new Category()
            {
                Id = Guid.Parse("db9b235f-a5d6-49dc-8e95-022f443f8582"),
                Name = "History",
                DisplayOrder = 3
            },
        };

        return categories;
    }
}
