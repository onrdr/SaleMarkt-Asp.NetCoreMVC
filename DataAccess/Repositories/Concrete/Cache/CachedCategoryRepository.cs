using Core.Utilities.Helpers;
using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly IMemoryCache _cache;
    private readonly CategoryRepository _decorated;
    private static readonly List<string> CachedKeys = new();
    private readonly CustomExpressionVisitor _customExpressionVisitor;

    public CachedCategoryRepository(
        CategoryRepository decorated, 
        IMemoryCache cache,
        CustomExpressionVisitor customExpressionVisitor)
    {
        _decorated = decorated;
        _cache = cache;
        _customExpressionVisitor = customExpressionVisitor;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        string key = $"category-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetByIdAsync(id);
        });
    } 

    public async Task<IEnumerable<Category>?> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        string key = $"all-categories-{GetPredicateKey(predicate)}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllAsync(predicate);
        });
    }

    public async Task<IEnumerable<Category>?> GetAllCategoriesWithProductsAsync(Expression<Func<Category, bool>> predicate)
    {
        string key = $"all-categories-with-product-{GetPredicateKey(predicate)}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllCategoriesWithProductsAsync(predicate);
        });
    }

    public async Task<int> AddAsync(Category entity)
    {
        int result = await _decorated.AddAsync(entity);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<Category> entities)
    {
        var result = await _decorated.AddRangeAsync(entities);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> UpdateAsync(Category entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var result = await _decorated.DeleteAsync(id);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Category> entities)
    {
        var result = await _decorated.DeleteRangeAsync(entities);
        RemoveAllCachedItems(result);
        return result;
    }

    #region Helper Methods
    private void RemoveAllCachedItems(int result)
    {
        if (result > 0)
        {
            foreach (var key in CachedKeys)
            {
                _cache.Remove(key);
            }
        }

        CachedKeys.Clear();
    }

    private string GetPredicateKey(Expression<Func<Category, bool>> predicate)
    {
        _customExpressionVisitor.Visit(predicate);
        var key = $"{predicate}_{string.Join("_", _customExpressionVisitor.Values)}";
        return key;
    }
    #endregion
}
