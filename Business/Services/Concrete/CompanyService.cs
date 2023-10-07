using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels; 

namespace Business.Services.Concrete;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository; 

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository; 
    }

    #region Read
    public async Task<IDataResult<Company>> GetCompanyAsync()
    {
        var companyList = await _companyRepository.GetAllAsync(c => true);
        return companyList.First() == null
            ? new ErrorDataResult<Company>(Messages.CompanyNotFound)
            : new SuccessDataResult<Company>(companyList.First());
    } 
    #endregion

    #region Update
    public async Task<IResult> UpdateCompany(CompanyViewModel model)
    {
        var companyResult = await GetCompanyAsync();
        if (!companyResult.Success)
        {
            return companyResult;
        }

        CompleteUpdate(model, companyResult);
        var updateResult = await _companyRepository.UpdateAsync(companyResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.CompanyUpdateSuccessfull)
            : new ErrorResult(Messages.CompanyUpdateError);
    } 

    private static void CompleteUpdate(CompanyViewModel model, IDataResult<Company> companyResult)
    {
        companyResult.Data.Name = model.Name;
        companyResult.Data.Email = model.Email;
        companyResult.Data.PhoneNumber = model.PhoneNumber; 
        companyResult.Data.Address = model.Address; 
        companyResult.Data.City = model.City; 
        companyResult.Data.Country = model.Country; 
        companyResult.Data.PostalCode = model.PostalCode;
        companyResult.Data.ImageUrl = model.ImageUrl; 
    }
    #endregion 
}
