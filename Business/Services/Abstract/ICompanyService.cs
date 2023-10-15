using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;

namespace Business.Services.Abstract;

public interface ICompanyService
{
    Task<IDataResult<Company>> GetCompanyAsync(); 
    Task<IResult> UpdateCompanyAsync(CompanyViewModel model);
}
