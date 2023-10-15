using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IProductService
{
    Task<IDataResult<Product>> GetByIdAsync(Guid productId);

    Task<IDataResult<Product>> GetProductWithCategoryAsync(Guid id);

    Task<IDataResult<IEnumerable<Product>>> GetAllAsync(Expression<Func<Product, bool>> predicate);

    Task<IDataResult<IEnumerable<Product>>> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate);

    Task<IResult> CreateProductAsync(ProductViewModel model);

    Task<IResult> UpdateProductAsync(ProductViewModel model);

    Task<IResult> DeleteProductAsync(Guid categoryId);
}
