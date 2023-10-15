using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IShoppingCartService
{
    Task<IDataResult<ShoppingCart>> GetByIdAsync(Guid shoppingCartId);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate);
    Task<IResult> CreateShoppingCartAsync(ShoppingCart model);
    Task<IResult> UpdateShoppingCartAsync(ShoppingCart model);
    Task<IResult> UpdateShoppingCartCountAsync(UpdatedCartItem updatedCart);
    Task<IResult> DeleteShoppingCartAsync(Guid shoppingCartId);
    Task<IResult> DeleteShoppingCartRangeAsync(IEnumerable<ShoppingCart> cartList);
    Task<int> GetItemCountForUserAsync(Guid appUserId);
}