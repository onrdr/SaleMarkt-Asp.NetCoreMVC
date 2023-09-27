using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

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
}