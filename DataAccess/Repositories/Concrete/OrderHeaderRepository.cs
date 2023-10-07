using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class OrderHeaderRepository : BaseRepository<OrderHeader>, IOrderHeaderRepository
{
    public OrderHeaderRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<OrderHeader?> GetByIdWithAppUserAsync(Guid orderHeaderId)
    {
        return await _dataContext.OrderHeaders
            .Include(x => x.AppUser) 
            .FirstOrDefaultAsync(x => x.Id == orderHeaderId);
    }

    public async Task<IEnumerable<OrderHeader>> GetAllWithAppUserAsync(Expression<Func<OrderHeader, bool>> predicate)
    {
        return await _dataContext.OrderHeaders
            .Include(x => x.AppUser)
            .Where(predicate)
            .ToListAsync();
    } 
}
