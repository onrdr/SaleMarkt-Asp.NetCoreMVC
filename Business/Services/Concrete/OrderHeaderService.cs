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

    public async Task<IDataResult<OrderHeader>> GetByIdAsync(Guid orderHeaderId)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdAsync(orderHeaderId);
        return orderHeader is null
            ? new ErrorDataResult<OrderHeader>(Messages.OrderHeaderNotFound)
            : new SuccessDataResult<OrderHeader>(orderHeader);
    }

    public async Task<IDataResult<OrderHeader>> GetByIdWithAppUserAsync(Guid orderHeaderId)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithAppUserAsync(orderHeaderId);
        return orderHeader is null
            ? new ErrorDataResult<OrderHeader>(Messages.OrderHeaderNotFound)
            : new SuccessDataResult<OrderHeader>(orderHeader);
    }

    public async Task<IDataResult<IEnumerable<OrderHeader>>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate)
    {
        var orderHeaderList = await _orderHeaderRepository.GetAllWithAppUserAsync(predicate);
        return orderHeaderList.Any()
            ? new SuccessDataResult<IEnumerable<OrderHeader>>(orderHeaderList)
            : new ErrorDataResult<IEnumerable<OrderHeader>>(Messages.EmptyOrderHeaderList);
    }

    public async Task<IResult> CreateOrderHeaderAsync(OrderHeader model)
    {
        var addResult = await _orderHeaderRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderHeaderAddSuccessfull)
            : new ErrorResult(Messages.OrderHeaderAddError);
    }

    public async Task<IResult> UpdateOrderStatus(Guid orderHeaderId, string orderStatus)
    {
        var orderHeaderResult = await GetByIdAsync(orderHeaderId); 
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
    public async Task<IResult> UpdatePaymentStatus(Guid orderHeaderId, string paymentStatus)
    {
        var orderHeaderResult = await GetByIdAsync(orderHeaderId);
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
}
