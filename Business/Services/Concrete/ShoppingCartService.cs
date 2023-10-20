using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;
using System.Linq.Expressions;
using Core.Utilities.Results;
using Core.Constants;
using AutoMapper;
using Models.Entities.Concrete;
using Models.ViewModels;

namespace Business.Services.Concrete;

public class ShoppingCartService : IShoppingCartService
{ 
    private readonly IShoppingCartRepository _shoppingCartRepository; 

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    #region Read
    public async Task<IDataResult<ShoppingCart>> GetShoppingCartByIdAsync(Guid shoppingCartId)
    {
        var shoppingCart = await _shoppingCartRepository.GetByIdAsync(shoppingCartId);
        return shoppingCart is not null
            ? new SuccessDataResult<ShoppingCart>(shoppingCart)
            : new ErrorDataResult<ShoppingCart>(Messages.ShoppingCartNotFound);
    }

    public async Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllShoppingCartsAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        var shoppingCartList = await _shoppingCartRepository.GetAllAsync(predicate);
        return shoppingCartList is not null && shoppingCartList.Any()
            ? new SuccessDataResult<IEnumerable<ShoppingCart>>(shoppingCartList)
            : new ErrorDataResult<IEnumerable<ShoppingCart>>(Messages.EmptyShoppingCartList);
    }

    public async Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllShoppingCartsWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        var shoppingCartList = await _shoppingCartRepository.GetAllWithProductAsync(predicate);
        return shoppingCartList is not null && shoppingCartList.Any()
            ? new SuccessDataResult<IEnumerable<ShoppingCart>>(shoppingCartList)
            : new ErrorDataResult<IEnumerable<ShoppingCart>>(Messages.EmptyShoppingCartList);
    }

    public async Task<int> GetShoppingCartItemCountByAppUserIdAsync(Guid appUserId)
    {
        var shoppingCartList = await _shoppingCartRepository.GetAllAsync(sc => sc.AppUserId == appUserId);
        return shoppingCartList is not null && shoppingCartList.Any() 
            ? shoppingCartList.Count() 
            : 0;
    }
    #endregion

    #region Create
    public async Task<IResult> CreateShoppingCartAsync(ShoppingCart model)
    {
        var addResult = await _shoppingCartRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.ShoppingCartAddSuccessfull)
            : new ErrorResult(Messages.ShoppingCartAddError);
    }

    #endregion

    #region Update
    public async Task<IResult> UpdateShoppingCartAsync(ShoppingCart model)
    {
        var shoppingCartResult = await GetShoppingCartByIdAsync(model.Id);
        if (!shoppingCartResult.Success)
        {
            return shoppingCartResult;
        }

        CompleteUpdate(model, shoppingCartResult.Data);
        return await GetUpdateResultAsync(shoppingCartResult);
    }

    private async Task<IResult> GetUpdateResultAsync(IDataResult<ShoppingCart> shoppingCartResult)
    {
        var updateResult = await _shoppingCartRepository.UpdateAsync(shoppingCartResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.ShoppingCartUpdateSuccessfull)
            : new ErrorResult(Messages.ShoppingCartUpdateError);
    }

    private static void CompleteUpdate(ShoppingCart model, ShoppingCart entity)
    { 
        model.Price = entity.Price;
        model.AppUser = entity.AppUser;
        model.AppUserId = entity.AppUserId;
        model.Product = entity.Product;
        model.ProductId = entity.ProductId;
        model.Count = entity.Count; 
    }

    public async Task<IResult> UpdateShoppingCartCountAsync(UpdatedCartItem updatedCart)
    {
        var shoppingCartResult = await GetShoppingCartByIdAsync(updatedCart.CartId);
        if (!shoppingCartResult.Success)
        {
            return shoppingCartResult;
        }

        shoppingCartResult.Data.Count = updatedCart.NewCount;
        var updateResult = await _shoppingCartRepository.UpdateAsync(shoppingCartResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.ShoppingCartUpdateSuccessfull)
            : new ErrorResult(Messages.ShoppingCartUpdateError);
    }
    #endregion 

    #region Delete
    public async Task<IResult> DeleteShoppingCartAsync(Guid shoppingCartId)
    {
        var deleteResult = await _shoppingCartRepository.DeleteAsync(shoppingCartId);
        return deleteResult > 0
            ? new SuccessResult(Messages.ShoppingCartDeleteSuccessfull)
            : new ErrorResult(Messages.ShoppingCartDeleteError);
    }

    public async Task<IResult> DeleteShoppingCartRangeAsync(IEnumerable<ShoppingCart> cartList)
    {
        var deleteResult = await _shoppingCartRepository.DeleteRangeAsync(cartList);
        return deleteResult > 0
            ? new SuccessResult(Messages.ShoppingCartDeleteSuccessfull)
            : new ErrorResult(Messages.ShoppingCartDeleteError);
    }
    #endregion
}
