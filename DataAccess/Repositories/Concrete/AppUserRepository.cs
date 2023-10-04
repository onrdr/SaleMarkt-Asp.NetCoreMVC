using DataAccess.Repositories.Abstract;
using Models.Identity;

namespace DataAccess.Repositories.Concrete;

public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository 
{
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
