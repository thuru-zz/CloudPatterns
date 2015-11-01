using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CacheAsidePatternWebTemplate.Cache
{
    public abstract class CacheProvider<ICachable>
    {
        public abstract Task<bool> SetItemAsync(string key, ICachable value, double ? time = null);

        public abstract Task<bool> SetCollectionAsync(string key, List<ICachable> values, double? time = null, int? length = null);

        public abstract Task<bool> InvalidateItemAsync(string key);

        public abstract Task<ICachable> GetItemAsync(string key);

        public abstract Task<List<ICachable>> GetCollectionForKeyAsync(string key);
    }
}