using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CacheAsidePatternWebTemplate.Cache
{
    public abstract class CacheProvider<T> where T : ICacheable
    {
        public abstract Task<bool> SetItemAsync(string key, T value);

        public abstract Task<bool> SetCollectionAsync(string key, List<T> value);

        public abstract Task<bool> InvalidateItemAsync(string key);

        public abstract Task<T> GetItemAsync(string key);

        public abstract Task<List<T>> GetCollectionForKeyAsync(string key);
    }
}