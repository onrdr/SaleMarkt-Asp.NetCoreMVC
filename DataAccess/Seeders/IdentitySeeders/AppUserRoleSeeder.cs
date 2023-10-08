using Microsoft.EntityFrameworkCore;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders;

public static class AppUserRoleSeeder
{
    public static ModelBuilder SeedAppUserRoles(this ModelBuilder modelBuilder)
    {
        var appUserRoles = GetAppUserRoles();
        foreach (var userRole in appUserRoles)
        {
            modelBuilder.Entity<AppUserRole>().HasData(userRole);
        }
        return modelBuilder;
    }

    public static List<AppUserRole> GetAppUserRoles()
    {
        var appUserRoles = new List<AppUserRole>()
        {
            new AppUserRole()
            {
               UserId = Guid.Parse("AD4C1F6B-F620-41D7-D3D8-08DBC6B87E76"),
               RoleId = Guid.Parse("41102F40-1CEE-4A61-9ADD-140D2608B1A5"),
            },
            new AppUserRole()
            {
               UserId = Guid.Parse("FD91A0BC-13E9-46D0-D3D7-08DBC6B87E76"),
               RoleId = Guid.Parse("2F514E34-8A22-4E36-AEFC-752BA3AA0B34"),
            },
            new AppUserRole()
            {
               UserId = Guid.Parse("E6218C9E-F224-46F1-A38E-08DBC6B81E6E"),
               RoleId = Guid.Parse("2B76647D-C501-44C3-91C7-A2BD6843B6E7"),
            },
        };

        return appUserRoles;
    }
}

