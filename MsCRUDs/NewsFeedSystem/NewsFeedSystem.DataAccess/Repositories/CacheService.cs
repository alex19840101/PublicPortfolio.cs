using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using NewsFeedSystem.Core.Repositories;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private const int EXPIRATION_DAYS = 366;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Получение объекта из кэша
        /// </summary>
        /// <typeparam name="T"> Класс кэшируемого объекта типа T </typeparam>
        /// <param name="key"> Ключ </param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = _cache.GetString(key);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value)!;
            }

            return default!;
        }

        /// <summary>
        /// Помещение объекта в кэш
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"> Ключ </param>
        /// <param name="value"> Кэшируемый объект типа T </param>
        /// <returns></returns>
        public T Set<T>(string key, T value)
        {
            _cache.SetString(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(EXPIRATION_DAYS),
            });
            return value;
        }
    }
}
