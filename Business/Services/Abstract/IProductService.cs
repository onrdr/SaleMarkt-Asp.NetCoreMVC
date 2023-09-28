using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface IProductService
{
    Task<IDataResult<Product>> GetByIdAsync(Guid productId);

    Task<IDataResult<Product>> GetProductWithCategory(Guid id);

    Task<IDataResult<IEnumerable<Product>>> GetAllAsync(Expression<Func<Product, bool>> predicate);

    Task<IDataResult<IEnumerable<Product>>> GetAllProductsWithCategory(Expression<Func<Product, bool>> predicate);

    Task<IResult> CreateProduct(ProductViewModel model);

    Task<IResult> UpdateProduct(ProductViewModel model);

    Task<IResult> DeleteProduct(Guid categoryId);
}
