using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Cnx.EarningsAndDeductions.Infrastructure.Cache
{

    public class MemoryCache : ICache
    {
 
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly IMemoryCache _cache;
 

        public MemoryCache()
        {

 
            _cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
 

        }

        public bool IsTracked(Guid id)
        {
 
            return _cache.TryGetValue(id, out var o) && o != null;
 
        }

        public void Set(Guid id, AggregateRoot aggregate)
        {
 
            _cache.Set(id, aggregate, _cacheOptions);
 
        }

        public AggregateRoot Get(Guid id)
        {
 
            return (AggregateRoot)_cache.Get(id);
 
        }

        public void Remove(Guid id)
        {
 
            _cache.Remove(id);
 
        }

        public void RegisterEvictionCallback(Action<Guid> action)
        {
 
            _cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                action.Invoke((Guid)key);
            });
 
        }
    }
}
