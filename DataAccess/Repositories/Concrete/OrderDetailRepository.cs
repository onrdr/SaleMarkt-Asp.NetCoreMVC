using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(ApplicationDbContext context) : base(context)
    {
        
    } 

    public async Task<IEnumerable<OrderDetail>> GetAllWithProductAsync(Expression<Func<OrderDetail, bool>> predicate)
    {
        return await _dataContext.OrderDetails
            .Include(o => o.Product)
            .Where(predicate) 
            .ToListAsync();
    }
}
