using DataAccess.Seeders.EntitySeeders;
using DataAccess.Seeders.IdentitySeeders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Seeders;

public static class DataSeeder
{
    public static ModelBuilder SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .SeedCategories()
            .SeedProducts()
            .SeedCompanies()
            .SeedRoles()
            .SeedAppUsers()
            .SeedAppUserRoles(); 

        return modelBuilder;
    }
}
