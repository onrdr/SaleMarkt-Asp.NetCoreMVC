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
        var category = await _orderHeaderRepository.GetByIdAsync(orderHeaderId);
        return category == null
            ? new ErrorDataResult<OrderHeader>(Messages.OrderHeaderFound)
            : new SuccessDataResult<OrderHeader>(category);
    }
    public async Task<IDataResult<IEnumerable<OrderHeader>>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate)
    {
        var categoryList = await _orderHeaderRepository.GetAllAsync(predicate);
        return categoryList.Any()
            ? new SuccessDataResult<IEnumerable<OrderHeader>>(categoryList)
            : new ErrorDataResult<IEnumerable<OrderHeader>>(Messages.EmptyOrderHeaderList);
    }

    public async Task<IResult> CreateOrderHeader(OrderHeader model)
    {
        var addResult = await _orderHeaderRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.OrderHeaderAddSuccessfull)
            : new ErrorResult(Messages.OrderHeaderAddError);
    }
}
