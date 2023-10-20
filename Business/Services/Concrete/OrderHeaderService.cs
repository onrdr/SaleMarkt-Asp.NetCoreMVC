using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class OrderHeaderService : IOrderHeaderService
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public OrderHeaderService(IOrderHeaderRepository orderHeaderRepository)
    {
        _orderHeaderRepository = orderHeaderRepository;
    }

    #region Read
    public async Task<IDataResult<OrderHeader>> GetOrderHeaderByIdAsync(Guid orderHeaderId)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdAsync(orderHeaderId);
        return orderHeader is null
            ? new ErrorDataResult<OrderHeader>(Messages.OrderHeaderNotFound)
            : new SuccessDataResult<OrderHeader>(orderHeader);
    }

    public async Task<IDataResult<OrderHeader>> GetOrderHeaderByIdWithAppUserAsync(Guid orderHeaderId)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithAppUserAsync(orderHeaderId);
        return orderHeader is null
            ? new ErrorDataResult<OrderHeader>(Messages.OrderHeaderNotFound)
            : new SuccessDataResult<OrderHeader>(orderHeader);
    }

    public async Task<IDataResult<IEnumerable<OrderHeader>>> GetAllOrderHeadersWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate)
    {
        var orderHeaderList = await _orderHeaderRepository.GetAllWithAppUserAsync(predicate);
        return orderHeaderList is not null && orderHeaderList.Any()
            ? new SuccessDataResult<IEnumerable<OrderHeader>>(orderHeaderList)
            : new ErrorDataResult<IEnumerable<OrderHeader>>(Messages.EmptyOrderHeaderList);
    }
    #endregion

    #region Create
    public async Task<IResult> CreateOrderHeaderAsync(OrderHeader model)
    {
        var addResult = await _orderHeaderRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderHeaderAddSuccessfull)
            : new ErrorResult(Messages.OrderHeaderAddError);
    }
    #endregion

    #region Update
    public async Task<IResult> UpdateOrderStatusAsync(Guid orderHeaderId, string orderStatus)
    {
        var orderHeaderResult = await GetOrderHeaderByIdAsync(orderHeaderId); 
        if (!orderHeaderResult.Success)
        {
            return orderHeaderResult;
        }

        orderHeaderResult.Data.OrderStatus = orderStatus;
        var updateResult = await _orderHeaderRepository.UpdateAsync(orderHeaderResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.OrderHeaderUpdateSuccessfull)
            : new ErrorResult(Messages.OrderHeaderUpdateError);
    }

    public async Task<IResult> UpdatePaymentStatusAsync(Guid orderHeaderId, string paymentStatus)
    {
        var orderHeaderResult = await GetOrderHeaderByIdAsync(orderHeaderId);
        if (!orderHeaderResult.Success)
        {
            return orderHeaderResult;
        }

        orderHeaderResult.Data.PaymentStatus = paymentStatus;
        var updateResult = await _orderHeaderRepository.UpdateAsync(orderHeaderResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.OrderHeaderUpdateSuccessfull)
            : new ErrorResult(Messages.OrderHeaderUpdateError);
    }

    public async Task<IResult> CompleteOrderAsync(Guid orderHeaderId)
    {
        var orderHeaderResult = await GetOrderHeaderByIdAsync(orderHeaderId);
        if (!orderHeaderResult.Success)
        {
            return orderHeaderResult;
        }

        orderHeaderResult.Data.OrderStatus = OrderHeaderStatus.Completed;
        orderHeaderResult.Data.PaymentStatus = OrderHeaderStatus.Completed;

        var updateResult = await _orderHeaderRepository.UpdateAsync(orderHeaderResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.OrderHeaderUpdateSuccessfull)
            : new ErrorResult(Messages.OrderHeaderUpdateError);
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteOrderAsync(Guid orderHeaderId)
    {
        var orderHeaderResult = await GetOrderHeaderByIdAsync(orderHeaderId);
        if (!orderHeaderResult.Success)
        {
            return orderHeaderResult;
        }

        var updateResult = await _orderHeaderRepository.DeleteAsync(orderHeaderId);
        return updateResult > 0
            ? new SuccessResult(Messages.OrderHeaderDeleteSuccessfull)
            : new ErrorResult(Messages.OrderHeaderDeleteError);
    }
    #endregion
}
