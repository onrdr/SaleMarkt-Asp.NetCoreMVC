using Core.Constants;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders;

public static class RoleSeeder
{
    public static ModelBuilder SeedRoles(this ModelBuilder modelBuilder)
    {
        var roles = GetRoles();
        foreach (var role in roles)
        {
            modelBuilder.Entity<AppRole>().HasData(role);
        }
        return modelBuilder;
    }
    public static List<AppRole> GetRoles()
    {
        var roles = new List<AppRole>()
        {
            new AppRole
            {
                Id = Guid.Parse("2b76647d-c501-44c3-91c7-a2bd6843b6e7"),
                Name = RoleNames.SuperAdmin,
                NormalizedName = RoleNames.SuperAdmin.ToUpper()
            },
            new AppRole
            {
                Id = Guid.Parse("2f514e34-8a22-4e36-aefc-752ba3aa0b34"),
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpper()
            },
            new AppRole
            {
                Id = Guid.Parse("41102f40-1cee-4a61-9add-140d2608b1a5"),
                Name = RoleNames.Customer,
                NormalizedName = RoleNames.Customer.ToUpper()
            }
        };

        return roles;
    }
}
