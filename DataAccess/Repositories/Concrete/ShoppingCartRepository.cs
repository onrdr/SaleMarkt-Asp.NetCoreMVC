using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class ShoppingCartRepository :  BaseRepository<ShoppingCart>, IShoppingCartRepository
{
    public ShoppingCartRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<ShoppingCart>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        return await _dataContext.ShoppingCarts
            .Where(predicate)
            .Include(s => s.Product)
            .ToListAsync();
    }
}
