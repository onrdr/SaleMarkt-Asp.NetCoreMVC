using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache;

public class CachedProductRepository : IProductRepository
{
    private readonly IMemoryCache _cache;
    private readonly ProductRepository _decorated;

    public CachedProductRepository(ProductRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        string key = $"product-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetByIdAsync(id);
        });
    }

    public async Task<Product?> GetProductWithCategoryAsync(Guid id)
    {
        string key = $"product-with-category-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetProductWithCategoryAsync(id);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllAsync(predicate);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products-with-category";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllProductsWithCategoryAsync(predicate);
        });
    }

    public async Task<int> AddAsync(Product entity)
    {
        int result = await _decorated.AddAsync(entity);
        RemoveAllCachedItems(entity.Id, result);
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<Product> entities)
    {
        var result = await _decorated.AddRangeAsync(entities);
        RemoveAllCachedItemRange(entities, result);
        return result;
    }

    public async Task<int> UpdateAsync(Product entity)
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

    public async Task<int> DeleteRangeAsync(IEnumerable<Product> entities)
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
            _cache.Remove($"product-{id}");
            _cache.Remove($"product-with-category-{id}");
            _cache.Remove($"all-products");
            _cache.Remove($"all-products-with-category");
        }
    }

    private void RemoveAllCachedItemRange(IEnumerable<Product> entities, int result)
    {
        if (result > 0)
        {
            foreach (var entity in entities)
            {
                _cache.Remove($"product-{entity.Id}");
                _cache.Remove($"product-with-category-{entity.Id}");
                _cache.Remove($"all-products");
                _cache.Remove($"all-products-with-category");
            }
        }
    }
    #endregion
}
