using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete; 

namespace DataAccess.Seeders;

public static class CompanySeeder
{
    public static ModelBuilder SeedCompanies(this ModelBuilder modelBuilder)
    {
        var companies = GetCompanies();
        foreach (var company in companies)
        {
            modelBuilder.Entity<Company>().HasData(company);
        }
        return modelBuilder;
    }

    public static List<Company> GetCompanies()
    {
        var companies = new List<Company>()
        {
            new Company()
            {
                Id = Guid.NewGuid(),
                Name = "SaleMarkt", 
                Email = "salemarkt@salemarkt.com",
                PhoneNumber = "1234567890",
                Address = "Beşiktaş",
                City = "Istanbul",
                Country = "Turkiye",
                PostalCode = "34000",
            }, 
        };

        return companies;
    }
}
