using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SeedData();
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products{ get; set; }
}