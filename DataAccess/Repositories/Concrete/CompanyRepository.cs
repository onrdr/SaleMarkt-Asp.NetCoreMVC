using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
