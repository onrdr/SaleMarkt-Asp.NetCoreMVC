using Core.Utilities.Helpers;
using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache;

public class CachedProductRepository : IProductRepository
{
    private readonly IMemoryCache _cache;
    private readonly ProductRepository _decorated;
    private readonly List<string> CachedKeys = new();
    private readonly CustomExpressionVisitor _customExpressionVisitor;

    public CachedProductRepository(
        ProductRepository decorated,
        IMemoryCache cache,
        CustomExpressionVisitor customExpressionVisitor)
    {
        _decorated = decorated;
        _cache = cache;
        _customExpressionVisitor = customExpressionVisitor;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        string key = $"product-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetByIdAsync(id);
        });
    }

    public async Task<Product?> GetProductWithCategoryAsync(Guid id)
    {
        string key = $"product-with-category-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetProductWithCategoryAsync(id);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products-{GetPredicateKey(predicate)}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllAsync(predicate);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products-with-category-{GetPredicateKey(predicate)}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllProductsWithCategoryAsync(predicate);
        });
    }

    public async Task<int> AddAsync(Product entity)
    {
        int result = await _decorated.AddAsync(entity);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<Product> entities)
    {
        var result = await _decorated.AddRangeAsync(entities);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> UpdateAsync(Product entity)
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

    public async Task<int> DeleteRangeAsync(IEnumerable<Product> entities)
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

    private string GetPredicateKey(Expression<Func<Product, bool>> predicate)
    {
        _customExpressionVisitor.Visit(predicate);
        var key = $"{predicate}_{string.Join("_", _customExpressionVisitor.Values)}";
        return key;
    }
    #endregion
}
