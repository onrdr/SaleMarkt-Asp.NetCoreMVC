using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository; 

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository; 
    }

    #region Read
    public async Task<IDataResult<Company>> GetByIdAsync(Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        return company == null
            ? new ErrorDataResult<Company>(Messages.CompanyNotFound)
            : new SuccessDataResult<Company>(company);
    }

    public async Task<IDataResult<IEnumerable<Company>>> GetAllAsync(Expression<Func<Company, bool>> predicate)
    {
        var companyList = await _companyRepository.GetAllAsync(predicate);
        return companyList.Any()
            ? new SuccessDataResult<IEnumerable<Company>>(companyList)
            : new ErrorDataResult<IEnumerable<Company>>(Messages.EmptyCompanyList);
    }
    #endregion

    #region Update
    public async Task<IResult> UpdateCompany(CompanyViewModel model)
    {
        var companyResult = await GetByIdAsync(model.Id);
        if (!companyResult.Success)
        {
            return companyResult;
        }

        CompleteUpdate(model, companyResult);
        return await GetUpdateResult(companyResult);
    }

    private async Task<IResult> GetUpdateResult(IDataResult<Company> companyResult)
    {
        var updateResult = await _companyRepository.UpdateAsync(companyResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.CompanyUpdateSuccessfull)
            : new ErrorResult(Messages.CompanyUpdateError);
    }

    private static void CompleteUpdate(CompanyViewModel model, IDataResult<Company> productResult)
    {
        productResult.Data.Name = model.Name;
        productResult.Data.Email = model.Email;
        productResult.Data.PhoneNumber = model.PhoneNumber; 
        productResult.Data.Address = model.Address; 
        productResult.Data.City = model.City; 
        productResult.Data.Country = model.Country; 
        productResult.Data.PostalCode = model.PostalCode; 
        productResult.Data.ImageUrl = model.ImageUrl; 
    }
    #endregion 
}
