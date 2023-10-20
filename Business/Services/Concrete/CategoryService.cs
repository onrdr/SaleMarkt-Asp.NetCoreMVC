using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;
using System.Linq.Expressions;
using Core.Utilities.Results;
using Core.Constants;
using AutoMapper;
using Models.ViewModels;
using Models.Entities.Concrete;

namespace Business.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    #region Read
    public async Task<IDataResult<Category>> GetCategoryByIdAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category is not null
            ? new SuccessDataResult<Category>(category)
            : new ErrorDataResult<Category>(Messages.CategoryNotFound);
    }

    public async Task<IDataResult<IEnumerable<Category>>> GetAllCategoriesAsync(Expression<Func<Category, bool>> predicate)
    {
        var categoryList = await _categoryRepository.GetAllAsync(predicate);
        return categoryList is not null && categoryList.Any()
            ? new SuccessDataResult<IEnumerable<Category>>(categoryList)
            : new ErrorDataResult<IEnumerable<Category>>(Messages.EmptyCategoryList);
    }
    #endregion

    #region Create
    public async Task<IResult> CreateCategoryAsync(CategoryViewModel model)
    {
        var existResult = await CheckIfCategoryNameAlreadyExistsAsync(model);
        if (!existResult.Success)
        {
            return existResult;
        }

        var addResult = await _categoryRepository.AddAsync(_mapper.Map<Category>(model));
        return addResult > 0
            ? new SuccessResult(Messages.CategoryAddSuccessfull)
            : new ErrorResult(Messages.CategoryAddError);
    }
    #endregion

    #region Update
    public async Task<IResult> UpdateCategoryAsync(CategoryViewModel model)
    {
        var categoryResult = await GetCategoryByIdAsync(model.Id);
        if (!categoryResult.Success)
        {
            return categoryResult;
        }

        var existResult = await CheckIfCategoryNameAlreadyExistsAsync(model);
        if (!existResult.Success)
        {
            return existResult;
        }

        CompleteUpdate(model, categoryResult);

        return await GetUpdateResultAsync(categoryResult);
    }

    private async Task<IResult> CheckIfCategoryNameAlreadyExistsAsync(CategoryViewModel model)
    {
        var categoryList = await _categoryRepository.GetAllAsync(c => true);
        if (categoryList is not null &&
            categoryList.Any(c => c.Name.ToLower() == model.Name.ToLower().Trim() && c.Id != model.Id))
        {
            return new ErrorResult(Messages.CategoryAlreadyExists);
        } 
        return new SuccessResult();
    } 

    private static void CompleteUpdate(CategoryViewModel model, IDataResult<Category> categoryResult)
    {
        categoryResult.Data.Name = model.Name;
        categoryResult.Data.DisplayOrder = model.DisplayOrder;
        categoryResult.Data.Description = model.Description;
        categoryResult.Data.ImageUrl = model.ImageUrl;
    }
    private async Task<IResult> GetUpdateResultAsync(IDataResult<Category> categoryResult)
    {
        var updateResult = await _categoryRepository.UpdateAsync(categoryResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.CategoryUpdateSuccessfull)
            : new ErrorResult(Messages.CategoryAddError);
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteCategoryAsync(Guid categoryId)
    {
        var deleteResult = await _categoryRepository.DeleteAsync(categoryId);
        return deleteResult > 0
            ? new SuccessResult(Messages.CategoryDeleteSuccessfull)
            : new ErrorResult(Messages.CategoryDeleteError);
    }
    #endregion
}
