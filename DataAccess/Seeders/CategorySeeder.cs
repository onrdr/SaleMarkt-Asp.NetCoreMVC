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
                Id = Guid.Parse("34245A4D-0BAA-4C22-8245-02ABB9063B11"),
                Name = "Action",
                DisplayOrder = 1,
                Description = "Category Action Description",
                ImageUrl = ""
            },
            new Category()
            {
                Id = Guid.Parse("76D6BE2F-8D6C-4E93-94CC-4EB0341950BC"),
                Name = "SciFi",
                DisplayOrder = 2,
                Description = "Category SciFi Description",
                ImageUrl = ""
            },
            new Category()
            {
                Id = Guid.Parse("DB9B235F-A5D6-49DC-8E95-022F443F8582"),
                Name = "History",
                DisplayOrder = 3,
                Description = "Category History Description",
                ImageUrl = ""
            },
             new Category()
            {
                Id = Guid.Parse("8179ED4D-7E5B-49C4-33F3-08DBC21909AC"),
                Name = "Crime",
                DisplayOrder = 4,
                Description = "Category Crime Description",
                ImageUrl = ""
            },
        };

        return categories;
    }
}
