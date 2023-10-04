using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IShoppingCartRepository : IBaseRepository<ShoppingCart>
{
    Task<IEnumerable<ShoppingCart>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate);
}
