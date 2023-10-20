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

    #region Read
    public async Task<IDataResult<OrderDetail>> GetOrderDetailByIdAsync(Guid orderDetailId)
    {
        var orderDetail = await _orderDetailsRepository.GetByIdAsync(orderDetailId);
        return orderDetail is not null
            ? new SuccessDataResult<OrderDetail>(orderDetail)
            : new ErrorDataResult<OrderDetail>(Messages.OrderDetailNotFound);
    }

    public async Task<IDataResult<IEnumerable<OrderDetail>>> GetAllOrderDetailsWithProductAsync(Expression<Func<OrderDetail, bool>> predicate)
    {
        var orderDetailList = await _orderDetailsRepository.GetAllWithProductAsync(predicate);
        return orderDetailList is not null && orderDetailList.Any()
            ? new SuccessDataResult<IEnumerable<OrderDetail>>(orderDetailList)
            : new ErrorDataResult<IEnumerable<OrderDetail>>(Messages.EmptyOrderDetailList);
    }
    #endregion

    #region Create
    public async Task<IResult> CreateOrderDetailAsync(OrderDetail model)
    {
        var addResult = await _orderDetailsRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderDetailAddSuccessfull)
            : new ErrorResult(Messages.OrderDetailAddError);
    } 
    #endregion
}
