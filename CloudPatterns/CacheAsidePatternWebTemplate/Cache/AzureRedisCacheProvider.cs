using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CacheAsidePatternWebTemplate.Cache
{
    public class AzureRedisCacheProvider : CacheProvider<ICacheable>
    {
        private readonly IDatabase _cache;
        private readonly CommandFlags _cachingStrategy;

        public AzureRedisCacheProvider(string connecitonString)
            : this(connecitonString, CommandFlags.FireAndForget)
        {
        }

        public AzureRedisCacheProvider(string connecitonString, CommandFlags cachingStrategy)
        {
            var connection = ConnectionMultiplexer.ConnectAsync(connecitonString).GetAwaiter().GetResult();

            _cache = connection.GetDatabase();
            _cachingStrategy = cachingStrategy;
        }

        public override async Task<bool> SetItemAsync(string key, ICacheable value, double? time = default(double?))
        {
            var jsonObject = JsonConvert.SerializeObject(value);

            if (time == null)
            {
                return await _cache.StringSetAsync(key, jsonObject, null, When.Always, _cachingStrategy);
            }
            else
            {
                return await _cache.StringSetAsync(key, jsonObject, TimeSpan.FromSeconds(time.Value), When.Always,
                    _cachingStrategy);
            }
        }

        public override async Task<ICacheable> GetItemAsync(string key)
        {
            var data = await _cache.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<ICacheable>(data);
            }
            throw new CacheProvderException("Not found or value null");
        }

        public override async Task<bool> SetCollectionAsync(string key, List<ICacheable> values, double? time = default(double?), int? length = default(int?))
        {
            if (values == null)
                throw new CacheProvderException("Collection cannot be null");

            try
            {
                var redisValues = new List<RedisValue>();

                foreach (var redisValue in values)
                {
                    redisValues.Add(JsonConvert.SerializeObject(redisValue));
                }

                await _cache.ListLeftPushAsync(key, redisValues.ToArray(), _cachingStrategy);

                if (length != null)
                    await _cache.ListTrimAsync(key, 0, length.Value, CommandFlags.FireAndForget);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<List<ICacheable>> GetCollectionForKeyAsync(string key)
        {
            var data = await _cache.ListRangeAsync(key, 0, -1, _cachingStrategy);
            var result = new List<ICacheable>();

            foreach (var item in data)
            {
                result.Add(JsonConvert.DeserializeObject<ICacheable>(item.ToString()));
            }

            return result;
        }

        public override async Task<bool> InvalidateItemAsync(string key)
        {
            return await _cache.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }
    }
}