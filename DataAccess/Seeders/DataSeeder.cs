using Microsoft.EntityFrameworkCore;

namespace DataAccess.Seeders;

public static class DataSeeder
{
    public static ModelBuilder SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .SeedCategories();

        return modelBuilder;
    }
}
