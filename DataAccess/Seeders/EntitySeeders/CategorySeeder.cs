using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

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
                Id = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                Name = "Category1",
                DisplayOrder = 1,
                Description = "Category1 Description",
                ImageUrl = ""
            },
            new Category()
            {
                Id = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                Name = "Category2",
                DisplayOrder = 2,
                Description = "Category2 Description",
                ImageUrl = ""
            },
            new Category()
            {
                Id = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                Name = "Category3",
                DisplayOrder = 3,
                Description = "Category3 Description",
                ImageUrl = ""
            },
             new Category()
            {
                Id = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                Name = "Category4",
                DisplayOrder = 4,
                Description = "Category4 Description",
                ImageUrl = ""
            },
        };

        return categories;
    }
}
