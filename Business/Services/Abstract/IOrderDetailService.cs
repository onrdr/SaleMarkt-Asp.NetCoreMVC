using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IOrderDetailService
{
    Task<IDataResult<OrderDetail>> GetOrderDetailByIdAsync(Guid orderDetailId);
    Task<IDataResult<IEnumerable<OrderDetail>>> GetAllOrderDetailsWithProductAsync(Expression<Func<OrderDetail, bool>> predicate);
    Task<IResult> CreateOrderDetailAsync(OrderDetail model); 
}