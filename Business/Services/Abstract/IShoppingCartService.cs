using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IShoppingCartService
{
    Task<IDataResult<ShoppingCart>> GetByIdAsync(Guid shoppingCartId);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IResult> CreateShoppingCart(ShoppingCart model);
    Task<IResult> UpdateShoppingCart(ShoppingCart model);
    Task<IResult> UpdateShoppingCartCount(UpdatedCartItem updatedCart);
    Task<IResult> DeleteShoppingCart(Guid shoppingCartId);
    Task<IResult> DeleteShoppingCartRange(IEnumerable<ShoppingCart> cartList);
    Task<int> GetItemCountForUserAsync(Guid appUserId);
}