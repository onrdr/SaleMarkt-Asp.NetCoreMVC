using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IOrderDetailsService
{
    Task<IDataResult<OrderDetails>> GetByIdAsync(Guid orderDetailsId);
    Task<IDataResult<IEnumerable<OrderDetails>>> GetAllWithAppUserAsync(Expression<Func<OrderDetails, bool>> predicate);
    Task<IResult> CreateOrderDetails(OrderDetails model); 
}
