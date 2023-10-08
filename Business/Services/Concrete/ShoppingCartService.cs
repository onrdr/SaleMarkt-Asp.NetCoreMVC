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
    public async Task<IDataResult<ShoppingCart>> GetByIdAsync(Guid shoppingCartId)
    {
        var category = await _shoppingCartRepository.GetByIdAsync(shoppingCartId);
        return category == null
            ? new ErrorDataResult<ShoppingCart>(Messages.ShoppingCartNotFound)
            : new SuccessDataResult<ShoppingCart>(category);
    }

    public async Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        var categoryList = await _shoppingCartRepository.GetAllAsync(predicate);
        return categoryList.Any()
            ? new SuccessDataResult<IEnumerable<ShoppingCart>>(categoryList)
            : new ErrorDataResult<IEnumerable<ShoppingCart>>(Messages.EmptyShoppingCartList);
    }

    public async Task<IDataResult<IEnumerable<ShoppingCart>>> GetAllWithProductAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        var categoryList = await _shoppingCartRepository.GetAllWithProductAsync(predicate);
        return categoryList.Any()
            ? new SuccessDataResult<IEnumerable<ShoppingCart>>(categoryList)
            : new ErrorDataResult<IEnumerable<ShoppingCart>>(Messages.EmptyShoppingCartList);
    }
    #endregion

    #region Create
    public async Task<IResult> CreateShoppingCart(ShoppingCart model)
    {
        var addResult = await _shoppingCartRepository.AddAsync(model);
        return addResult > 0
            ? new SuccessResult(Messages.ShoppingCartAddSuccessfull)
            : new ErrorResult(Messages.ShoppingCartAddError);
    }

    #endregion

    #region Update
    public async Task<IResult> UpdateShoppingCart(ShoppingCart model)
    {
        var shoppingCartResult = await GetByIdAsync(model.Id);
        if (!shoppingCartResult.Success)
        {
            return shoppingCartResult;
        }

        CompleteUpdate(model, shoppingCartResult.Data);
        return await GetUpdateResult(shoppingCartResult);
    }

    private async Task<IResult> GetUpdateResult(IDataResult<ShoppingCart> shoppingCartResult)
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

    public async Task<IResult> UpdateShoppingCartCount(UpdatedCartItem updatedCart)
    {
        var shoppingCartResult = await GetByIdAsync(updatedCart.CartId);
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
    public async Task<IResult> DeleteShoppingCart(Guid shoppingCartId)
    {
        var deleteResult = await _shoppingCartRepository.DeleteAsync(shoppingCartId);
        return deleteResult > 0
            ? new SuccessResult(Messages.ShoppingCartDeleteSuccessfull)
            : new ErrorResult(Messages.ShoppingCartDeleteError);
    }

    public async Task<IResult> DeleteShoppingCartRange(IEnumerable<ShoppingCart> cartList)
    {
        var deleteResult = await _shoppingCartRepository.DeleteRangeAsync(cartList);
        return deleteResult > 0
            ? new SuccessResult(Messages.ShoppingCartDeleteSuccessfull)
            : new ErrorResult(Messages.ShoppingCartDeleteError);
    }
    #endregion

    public async Task<int> GetItemCountForUserAsync(Guid appUserId)
    {
        var list = await _shoppingCartRepository.GetAllAsync(sc => sc.AppUserId == appUserId);
        return list.Count();
    }

}
