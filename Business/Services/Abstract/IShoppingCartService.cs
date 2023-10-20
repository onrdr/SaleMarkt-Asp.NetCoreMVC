using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IShoppingCartService
{
    Task<IDataResult<ShoppingCart>> GetShoppingCartByIdAsync(Guid shoppingCartId);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllShoppingCartsAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllShoppingCartsWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IResult> CreateShoppingCartAsync(ShoppingCart model);
    Task<IResult> UpdateShoppingCartAsync(ShoppingCart model);
    Task<IResult> UpdateShoppingCartCountAsync(UpdatedCartItem updatedCart);
    Task<IResult> DeleteShoppingCartAsync(Guid shoppingCartId);
    Task<IResult> DeleteShoppingCartRangeAsync(IEnumerable<ShoppingCart> cartList);
    Task<int> GetShoppingCartItemCountByAppUserIdAsync(Guid appUserId);
}