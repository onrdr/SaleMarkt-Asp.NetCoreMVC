using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<Product?> GetProductWithCategory(Guid id)
    {
        return await _dataContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>?> GetAllProductsWithCategory(Expression<Func<Product, bool>> predicate)
    {
        return await _dataContext.Products
            .Where(predicate)
            .Include(p => p.Category)
            .ToListAsync();
    }
}
