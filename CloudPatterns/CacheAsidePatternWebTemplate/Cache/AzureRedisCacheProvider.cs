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

        public override async Task<bool> SetItemAsync(string key, ICacheable value)
        {
            var jsonObject = JsonConvert.SerializeObject(value);
           
            if (value.LifeTime.HasValue)
            {
                return await _cache.StringSetAsync(key, jsonObject, TimeSpan.FromMinutes(value.LifeTime.Value), When.Always, 
                    _cachingStrategy);
            }
            else
            {
                return await _cache.StringSetAsync(key, jsonObject, null, When.Always, _cachingStrategy);
            }
        }

        public override async Task<ICacheable> GetItemAsync(string key)
        {
            var data = await _cache.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<ICacheable>(data);
            }
            return null;
        }

        public override Task<List<ICacheable>> GetCollectionForKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        

        public override Task<bool> InvalidateItemAsync(string key)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> SetCollectionAsync(string key, List<ICacheable> values)
        {
            if (values == null)
                throw new CacheProvderException("Value Null Exceptipn");

            try
            {
                var redisValues = new List<RedisValue>();

                foreach (var redisValue in values)
                {
                    redisValues.Add(JsonConvert.SerializeObject(redisValue));
                }

                await _cache.ListLeftPushAsync(key, redisValues.ToArray(), _cachingStrategy);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}