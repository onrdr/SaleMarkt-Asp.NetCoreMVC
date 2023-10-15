using Core.Constants;
using Microsoft.EntityFrameworkCore;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders;

public static class AppUserSeeder
{
    public static ModelBuilder SeedAppUsers(this ModelBuilder modelBuilder)
    {
        var appUsers = GetAppUsers();
        foreach (var appUser in appUsers)
        {
            modelBuilder.Entity<AppUser>().HasData(appUser);
        }
        return modelBuilder;
    }

    public static List<AppUser> GetAppUsers()
    {
        var appUsers = new List<AppUser>()
        {
            new AppUser()
            {
                Id = Guid.Parse("E6218C9E-F224-46F1-A38E-08DBC6B81E6E"),
                Name = "Super Admin",
                UserName = "Super-Admin",
                NormalizedUserName = "SUPER-ADMIN",
                Email = "superadmin@salemarkt.com",
                NormalizedEmail = "SUPERADMIN@SALEMARKT.COM",
                PasswordHash = "AQAAAAIAAYagAAAAECByl9RKmTARk5w1x7F8JgWCAT8zKI9y2s/BaQVHO2pPxMAncULF+44kC7fMz4nnpA==",
                SecurityStamp = "LB3ETSAI7Y2TFBWIODGRBQU774ITQI7G",
                ConcurrencyStamp = "a5b4ce0d-3189-4b16-a4df-9adbde35b40d",
                PhoneNumber = "1234567890",
                Address = "Levent",
                City = "Istanbul",
                Country = "Turkiye",
                PostalCode = "34000",
                Role = RoleNames.SuperAdmin
            },

            new AppUser()
            {
                Id = Guid.Parse("FD91A0BC-13E9-46D0-D3D7-08DBC6B87E76"),
                Name = "Company Admin",
                UserName = "Company-Admin",
                NormalizedUserName = "COMPANY-ADMIN",
                Email = "companyadmin@salemarkt.com",
                NormalizedEmail = "COMPANYADMIN@SALEMARKT.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEPvC/yQD0wrfDxCYISaEgR2+RfJQcGEJzK2PeseSSyDlpOj9cVEsz9oaCIEGHGw1ag==",
                SecurityStamp = "BZMCGV4LGIAVLGR4NXGRVTBCBKZNAZRK",
                ConcurrencyStamp = "156d7eb0-4752-4147-817a-90696985e4d5",
                PhoneNumber = "0987654321",
                Address = "Beşiktaş",
                City = "Istanbul",
                Country = "Turkiye",
                PostalCode = "34000",
                Role = RoleNames.Admin
            },

            new AppUser()
            {
                Id = Guid.Parse("AD4C1F6B-F620-41D7-D3D8-08DBC6B87E76"),
                Name = "Company Customer ",
                UserName = "Company-Customer",
                NormalizedUserName = "COMPANY-CUSTOMER",
                Email = "customer@salemarkt.com",
                NormalizedEmail = "CUSTOMER@SALEMARKT.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEEPT05PyWYpkO5AqVs4mvyalAB0VPjWhSknJ1HonAod5tewuuD5R9FtW2mO23/21XQ==",
                SecurityStamp = "PVNIK4FXHMWDXUJSOPGDPO4DPHTHOAM7",
                ConcurrencyStamp = "b124af6f-c7a3-4abc-a917-456602df151d",
                PhoneNumber = "1029384756",
                Address = "Şişli",
                City = "Istanbul",
                Country = "Turkiye",
                PostalCode = "34000",
                Role = RoleNames.Customer
            },
        };

        return appUsers;
    }
}
