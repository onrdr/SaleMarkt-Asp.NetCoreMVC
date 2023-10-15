using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface ICategoryService
{
    Task<IDataResult<Category>> GetByIdAsync(Guid categoryId);
    Task<IDataResult<IEnumerable<Category>>> GetAllAsync(Expression<Func<Category, bool>> predicate);
    Task<IResult> CreateCategoryAsync(CategoryViewModel model);
    Task<IResult> UpdateCategoryAsync(CategoryViewModel model);
    Task<IResult> DeleteCategoryAsync(Guid categoryId);
}
