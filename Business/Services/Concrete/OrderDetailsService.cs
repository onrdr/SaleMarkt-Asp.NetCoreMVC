using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class OrderDetailsService : IOrderDetailsService
{
    private readonly IOrderDetailsRepository _orderDetailsRepository;

    public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository)
    {
        _orderDetailsRepository = orderDetailsRepository;
    }

    public async Task<IDataResult<OrderDetails>> GetByIdAsync(Guid orderHeaderId)
    {
        var category = await _orderDetailsRepository.GetByIdAsync(orderHeaderId);
        return category == null
            ? new ErrorDataResult<OrderDetails>(Messages.OrderDetailsFound)
            : new SuccessDataResult<OrderDetails>(category);
    }
    public async Task<IDataResult<IEnumerable<OrderDetails>>> GetAllWithAppUserAsync(Expression<Func<OrderDetails, bool>> predicate)
    {
        var categoryList = await _orderDetailsRepository.GetAllAsync(predicate);
        return categoryList.Any()
            ? new SuccessDataResult<IEnumerable<OrderDetails>>(categoryList)
            : new ErrorDataResult<IEnumerable<OrderDetails>>(Messages.EmptyOrderDetailsList);
    }

    public async Task<IResult> CreateOrderDetails(OrderDetails model)
    {
        var addResult = await _orderDetailsRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderDetailsAddSuccessfull)
            : new ErrorResult(Messages.OrderDetailsAddError);
    } 
}
