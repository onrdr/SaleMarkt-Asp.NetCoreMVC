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
    Task<IResult> UpdateOrderStatusAsync(Guid orderHeaderId, string orderStatus); 
    Task<IResult> UpdatePaymentStatusAsync(Guid orderHeaderId, string paymentStatus); 
    Task<IResult> CompleteOrderAsync(Guid orderHeaderId);
    Task<IResult> DeleteOrderAsync(Guid orderHeaderId);
}
