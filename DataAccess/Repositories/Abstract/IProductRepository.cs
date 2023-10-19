using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductWithCategoryAsync(Guid id);
    Task<IEnumerable<Product>?> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate);
}
