using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract;

public interface ICompanyService
{
    Task<IDataResult<Company>> GetByIdAsync(Guid companyId);
    Task<IDataResult<IEnumerable<Company>>> GetAllAsync(Expression<Func<Company, bool>> predicate);
    Task<IResult> UpdateCompany(CompanyViewModel model);
}
