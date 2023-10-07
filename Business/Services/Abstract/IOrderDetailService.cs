using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IOrderDetailService
{
    Task<IDataResult<OrderDetail>> GetByIdAsync(Guid orderDetailId);
    Task<IDataResult<IEnumerable<OrderDetail>>> GetAllWithProductAsync(Expression<Func<OrderDetail, bool>> predicate);
    Task<IResult> CreateOrderDetails(OrderDetail model); 
}