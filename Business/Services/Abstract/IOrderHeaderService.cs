using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IOrderHeaderService
{
    Task<IDataResult<OrderHeader>> GetByIdAsync(Guid orderHeaderId);
    Task<IDataResult<IEnumerable<OrderHeader>>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate);
    Task<IResult> CreateOrderHeader(OrderHeader model); 
}
