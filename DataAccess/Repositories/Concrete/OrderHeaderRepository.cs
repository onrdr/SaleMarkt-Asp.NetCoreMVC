using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class OrderHeaderRepository : BaseRepository<OrderHeader>, IOrderHeaderRepository
{
    public OrderHeaderRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
