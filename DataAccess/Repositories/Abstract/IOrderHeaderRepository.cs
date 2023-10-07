using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IOrderHeaderRepository : IBaseRepository<OrderHeader>
{
    Task<IEnumerable<OrderHeader>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate);
    Task<OrderHeader?> GetByIdWithAppUserAsync(Guid orderHeaderId);

}
