using Core.Utilities.Results;
using Models.Entities;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface ICategoryService
{
    Task<IDataResult<Category>> GetByIdAsync(Guid categoryId);
    Task<IDataResult<IEnumerable<Category>>> GetAllAsync(Expression<Func<Category, bool>> predicate);
    Task<IResult> CreateCategory(CategoryViewModel model);
    Task<IResult> UpdateCategory(CategoryViewModel model);
    Task<IResult> DeleteCategory(Guid categoryId);
}
