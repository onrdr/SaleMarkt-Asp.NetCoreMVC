using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly IMemoryCache _cache;
    private readonly CategoryRepository _decorated;

    public CachedCategoryRepository(CategoryRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        string key = $"category-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetByIdAsync(id);
        });
    } 

    public async Task<IEnumerable<Category>?> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        string key = $"all-categories";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllAsync(predicate);
        });
    } 

    public async Task<int> AddAsync(Category entity)
    {
        int result = await _decorated.AddAsync(entity);
        RemoveAllCachedItems(entity.Id, result);
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<Category> entities)
    {
        var result = await _decorated.AddRangeAsync(entities);
        RemoveAllCachedItemRange(entities, result);
        return result;
    }
    public async Task<int> UpdateAsync(Category entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        RemoveAllCachedItems(entity.Id, result);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var result = await _decorated.DeleteAsync(id);
        RemoveAllCachedItems(id, result);
        return result;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Category> entities)
    {
        var result = await _decorated.DeleteRangeAsync(entities);
        RemoveAllCachedItemRange(entities, result);
        return result;
    }

    #region Helper Methods
    private void RemoveAllCachedItems(Guid id, int result)
    {
        if (result > 0)
        {
            _cache.Remove($"category-{id}"); 
            _cache.Remove($"all-categories"); 
        }
    }

    private void RemoveAllCachedItemRange(IEnumerable<Category> entities, int result)
    {
        if (result > 0)
        {
            foreach (var entity in entities) 
                _cache.Remove($"category-{entity.Id}");

            _cache.Remove($"all-categories");
        }
    }
    #endregion
}
