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

    public async Task<IDataResult<Category>> GetByIdAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category == null
            ? new ErrorDataResult<Category>(Messages.CategoryNotFound)
            : new SuccessDataResult<Category>(category);
    } 

    public async Task<IDataResult<IEnumerable<Category>>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        var categoryList = await _categoryRepository.GetAllAsync(predicate);
        return categoryList.Any()
            ? new SuccessDataResult<IEnumerable<Category>>(categoryList)
            : new ErrorDataResult<IEnumerable<Category>>(Messages.EmptyCategoryList);
    }

    public async Task<IResult> CreateCategory(CategoryViewModel model)
    {        
        var categoryList = await _categoryRepository.GetAllAsync(c => true); 
        if (categoryList.Any(c => c.Name.ToLower() == model.Name.ToLower().Trim()))
        {
            return new ErrorResult(Messages.CategoryAlreadyExists);
        } 
        
        var addResult = await _categoryRepository.AddAsync(_mapper.Map<Category>(model));
        return addResult > 0
            ? new SuccessResult(Messages.CategoryAddSuccessfull)
            : new ErrorResult(Messages.CategoryAddError);
    }

    public async Task<IResult> UpdateCategory(CategoryViewModel model)
    {
        var categoryResult = await GetByIdAsync(model.Id);
        if (!categoryResult.Success)
        {
            return categoryResult;
        }

        var categoryList = await _categoryRepository.GetAllAsync(c => true);
        if (categoryList.Contains(categoryResult.Data))
        { 
            var list = categoryList.ToList();
            list.Remove(categoryResult.Data);

            if (list.Any(c => c.Name.ToLower() == model.Name.ToLower().Trim()))
            {
                return new ErrorResult(Messages.CategoryAlreadyExists);
            }
        }

        categoryResult.Data.Name = model.Name;
        categoryResult.Data.DisplayOrder = model.DisplayOrder;

        var updateResult = await _categoryRepository.UpdateAsync(categoryResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.CategoryUpdateSuccessfull)
            : new ErrorResult(Messages.CategoryAddError);
    }

    public async Task<IResult> DeleteCategory(Guid categoryId)
    {
        var deleteResult = await _categoryRepository.DeleteAsync(categoryId);
        return deleteResult > 0
            ? new SuccessResult(Messages.CategoryDeleteSuccessfull)
            : new ErrorResult(Messages.CategoryAddError);
    }
}
