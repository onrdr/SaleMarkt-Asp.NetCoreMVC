using Microsoft.EntityFrameworkCore;
using Models.Identity;

namespace DataAccess.Seeders;

public static class RoleSeeder
{
    public static ModelBuilder SeedRoles(this ModelBuilder modelBuilder)
    { 
        string[] rolesToSeed = { "SuperAdmin", "Admin", "Customer" };
         
        foreach (var roleName in rolesToSeed)
        {
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                }
            );
        }

        return modelBuilder;
    }
}
