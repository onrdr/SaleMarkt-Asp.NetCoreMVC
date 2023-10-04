using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IShoppingCartService
{
    Task<IDataResult<ShoppingCart>> GetByIdAsync(Guid shoppingCartId);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IResult> CreateShoppingCart(ShoppingCart model);
    Task<IResult> UpdateShoppingCart(ShoppingCart model);
    Task<IResult> DeleteShoppingCart(Guid shoppingCartId);
    Task<int> GetItemCountForUserAsync(Guid appUserId);
}