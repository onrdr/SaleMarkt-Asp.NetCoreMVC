using DataAccess.Repositories.Abstract;
using Models.Entities;

namespace DataAccess.Repositories.Concrete;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
