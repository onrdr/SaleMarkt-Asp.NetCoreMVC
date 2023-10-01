using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    #region Read
    public async Task<IDataResult<Product>> GetByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product == null
            ? new ErrorDataResult<Product>(Messages.ProductNotFound)
            : new SuccessDataResult<Product>(product);
    }

    public async Task<IDataResult<Product>> GetProductWithCategory(Guid id)
    {
        var product = await _productRepository.GetProductWithCategory(id);
        return product == null
           ? new ErrorDataResult<Product>(Messages.ProductNotFound)
           : new SuccessDataResult<Product>(product);
    }

    public async Task<IDataResult<IEnumerable<Product>>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        var productList = await _productRepository.GetAllAsync(predicate);
        return productList.Any()
            ? new SuccessDataResult<IEnumerable<Product>>(productList)
            : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
    }

    public async Task<IDataResult<IEnumerable<Product>>> GetAllProductsWithCategory(Expression<Func<Product, bool>> predicate)
    {
        var productList = await _productRepository.GetAllProductsWithCategory(predicate);
        return productList.Any()
            ? new SuccessDataResult<IEnumerable<Product>>(productList)
            : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
    }

    #endregion

    #region Create
    public async Task<IResult> CreateProduct(ProductViewModel model)
    {
        var addResult = await _productRepository.AddAsync(_mapper.Map<Product>(model));
        return addResult > 0
            ? new SuccessResult(Messages.ProductAddSuccessfull)
            : new ErrorResult(Messages.ProductAddError);
    }

    #endregion

    #region Update
    public async Task<IResult> UpdateProduct(ProductViewModel model)
    {
        var productResult = await GetByIdAsync(model.Id);
        if (!productResult.Success)
        {
            return productResult;
        }

        CompleteUpdate(model, productResult);
        return await GetUpdateResult(productResult);
    }

    private async Task<IResult> GetUpdateResult(IDataResult<Product> productResult)
    {
        var updateResult = await _productRepository.UpdateAsync(productResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.ProductUpdateSuccessfull)
            : new ErrorResult(Messages.ProductAddError);
    }

    private static void CompleteUpdate(ProductViewModel model, IDataResult<Product> productResult)
    {
        productResult.Data.Title = model.Title;
        productResult.Data.Author = model.Author;
        productResult.Data.Description = model.Description;
        productResult.Data.ISBN = model.ISBN;
        productResult.Data.ListPrice = model.ListPrice;
        productResult.Data.Price = model.Price;
        productResult.Data.Price50 = model.Price50;
        productResult.Data.Price100 = model.Price100;
        productResult.Data.CategoryId = model.CategoryId;
        productResult.Data.ImageUrl = model.ImageUrl ?? productResult.Data.ImageUrl;

    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteProduct(Guid productId)
    {
        var deleteResult = await _productRepository.DeleteAsync(productId);
        return deleteResult > 0
            ? new SuccessResult(Messages.ProductDeleteSuccessfull)
            : new ErrorResult(Messages.ProductAddError);
    }
    #endregion 
}
