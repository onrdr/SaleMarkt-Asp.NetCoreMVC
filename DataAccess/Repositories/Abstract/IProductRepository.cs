using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductWithCategory(Guid id);
    Task<IEnumerable<Product>?> GetAllProductsWithCategory(Expression<Func<Product, bool>> predicate);
}
