using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
{
    Task<IEnumerable<OrderDetail>> GetAllWithProductAsync(Expression<Func<OrderDetail, bool>> predicate); 
}
