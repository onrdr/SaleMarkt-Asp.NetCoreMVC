using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Category>?> GetAllCategoriesWithProductsAsync(Expression<Func<Category, bool>> predicate)
    {
        var categories = await _dataContext.Categories
            .Where(predicate)
            .Include(c => c.Products)
            .ToListAsync();

        return categories;
    }
}
