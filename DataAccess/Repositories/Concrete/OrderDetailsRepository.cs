using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class OrderDetailsRepository : BaseRepository<OrderDetails>, IOrderDetailsRepository
{
    public OrderDetailsRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
