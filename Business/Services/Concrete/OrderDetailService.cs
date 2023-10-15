using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract; 
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class OrderDetailService : IOrderDetailService
{
    private readonly IOrderDetailRepository _orderDetailsRepository;

    public OrderDetailService(IOrderDetailRepository orderDetailsRepository)
    {
        _orderDetailsRepository = orderDetailsRepository;
    }

    public async Task<IDataResult<OrderDetail>> GetByIdAsync(Guid orderHeaderId)
    {
        var category = await _orderDetailsRepository.GetByIdAsync(orderHeaderId);
        return category == null
            ? new ErrorDataResult<OrderDetail>(Messages.OrderDetailNotFound)
            : new SuccessDataResult<OrderDetail>(category);
    }

    public async Task<IDataResult<IEnumerable<OrderDetail>>> GetAllWithProductAsync(Expression<Func<OrderDetail, bool>> predicate)
    {
        var orderDetailList = await _orderDetailsRepository.GetAllWithProductAsync(predicate);
        return !orderDetailList.Any()
            ? new ErrorDataResult<IEnumerable<OrderDetail>>(Messages.EmptyOrderDetailList)
            : new SuccessDataResult<IEnumerable<OrderDetail>>(orderDetailList);
    } 

    public async Task<IResult> CreateOrderDetailsAsync(OrderDetail model)
    {
        var addResult = await _orderDetailsRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderDetailAddSuccessfull)
            : new ErrorResult(Messages.OrderDetailAddError);
    } 
}
