﻿using AutoMapper;
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
    public async Task<IDataResult<Product>> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product is not null
            ? new SuccessDataResult<Product>(product)
            : new ErrorDataResult<Product>(Messages.ProductNotFound);
    }

    public async Task<IDataResult<Product>> GetProductByIdWithCategoryAsync(Guid productId)
    {
        var product = await _productRepository.GetProductWithCategoryAsync(productId);
        return product is not null
           ? new SuccessDataResult<Product>(product)
           : new ErrorDataResult<Product>(Messages.ProductNotFound);
    }

    public async Task<IDataResult<IEnumerable<Product>>> GetAllProductsAsync(Expression<Func<Product, bool>> predicate)
    {
        var productList = await _productRepository.GetAllAsync(predicate);
        return productList is not null && productList.Any()
            ? new SuccessDataResult<IEnumerable<Product>>(productList)
            : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
    }

    public async Task<IDataResult<IEnumerable<Product>>> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
    {
        var productList = await _productRepository.GetAllProductsWithCategoryAsync(predicate);
        return productList is not null && productList.Any()
            ? new SuccessDataResult<IEnumerable<Product>>(productList)
            : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
    }

    #endregion

    #region Create
    public async Task<IResult> CreateProductAsync(ProductViewModel model)
    {
        var addResult = await _productRepository.AddAsync(_mapper.Map<Product>(model));
        return addResult > 0
            ? new SuccessResult(Messages.ProductAddSuccessfull)
            : new ErrorResult(Messages.ProductAddError);
    }

    #endregion

    #region Update
    public async Task<IResult> UpdateProductAsync(ProductViewModel model)
    {
        var productResult = await GetProductByIdAsync(model.Id);
        if (!productResult.Success)
        {
            return productResult;
        }

        CompleteUpdate(model, productResult);
        return await GetUpdateResultAsync(productResult);
    }

    private async Task<IResult> GetUpdateResultAsync(IDataResult<Product> productResult)
    {
        var updateResult = await _productRepository.UpdateAsync(productResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.ProductUpdateSuccessfull)
            : new ErrorResult(Messages.ProductUpdateError);
    }

    private static void CompleteUpdate(ProductViewModel model, IDataResult<Product> productResult)
    {
        productResult.Data.Title = model.Title;
        productResult.Data.Description = model.Description;
        productResult.Data.Color = model.Color;
        productResult.Data.ListPrice = model.ListPrice;
        productResult.Data.Price = model.Price;
        productResult.Data.Price50 = model.Price50;
        productResult.Data.Price100 = model.Price100;
        productResult.Data.CategoryId = model.CategoryId;
        productResult.Data.ImageUrl = model.ImageUrl ?? productResult.Data.ImageUrl;

    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteProductAsync(Guid productId)
    {
        var deleteResult = await _productRepository.DeleteAsync(productId);
        return deleteResult > 0
            ? new SuccessResult(Messages.ProductDeleteSuccessfull)
            : new ErrorResult(Messages.ProductDeleteError);
    }
    #endregion 
}
