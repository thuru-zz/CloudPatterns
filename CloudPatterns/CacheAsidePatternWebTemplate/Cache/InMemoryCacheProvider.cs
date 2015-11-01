using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CacheAsidePatternWebTemplate.Cache
{
    public class InMemoryCacheProvider : CacheProvider<ICacheable>
    {
        public override Task<List<ICacheable>> GetCollectionForKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public override Task<ICacheable> GetItemAsync(string key)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> InvalidateItemAsync(string key)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> SetCollectionAsync(string key, List<ICacheable> values, double? time = default(double?), int? length = default(int?))
        {
            throw new NotImplementedException();
        }

        public override Task<bool> SetItemAsync(string key, ICacheable value, double? time = default(double?))
        {
            throw new NotImplementedException();
        }
    }
}