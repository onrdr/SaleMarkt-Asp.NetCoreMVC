using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IOrderHeaderService
{
    Task<IDataResult<OrderHeader>> GetByIdAsync(Guid orderHeaderId);
    Task<IDataResult<OrderHeader>> GetByIdWithAppUserAsync(Guid orderHeaderId);
    Task<IDataResult<IEnumerable<OrderHeader>>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate);
    Task<IResult> CreateOrderHeaderAsync(OrderHeader model); 
    Task<IResult> UpdateOrderStatus(Guid orderHeaderId, string orderStatus); 
    Task<IResult> UpdatePaymentStatus(Guid orderHeaderId, string paymentStatus); 
}
