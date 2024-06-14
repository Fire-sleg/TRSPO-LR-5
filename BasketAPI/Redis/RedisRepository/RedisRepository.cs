using BasketAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BasketAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;
using System.Reflection;
using BasketAPI.Redis.IRepository;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace BasketAPI.Redis.RedisRepository
{
    public class RedisRepository : IRepository.IRepository
    {
        private readonly IDatabase _redisDatabase;
        private readonly ApplicationDbContext _db;
        internal DbSet<Product> _dbSet;

        public RedisRepository(IDatabase redisDatabase, ApplicationDbContext db)
        {
            _redisDatabase = redisDatabase;
            _db = db;
            _dbSet = db.Set<Product>();
        }
        public async Task CreateAsync(Product entity)
        {
            //Redis
            var cacheKey = GenerateCacheKey(entity);
            var serializedData = System.Text.Json.JsonSerializer.Serialize(entity);
            //await _cache.SetStringAsync(cacheKey, serializedData);
            await _redisDatabase.StringSetAsync(cacheKey, serializedData, TimeSpan.FromMinutes(5));


            //БД
            //await _dbSet.AddAsync(entity);
            //await SaveAsync();
        }
        public async Task<Product> GetAsync(Guid id, bool tracked = true)
        {
            string key = GenerateCacheKey(id.ToString());
            var value = await _redisDatabase.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return null;
            return System.Text.Json.JsonSerializer.Deserialize<Product>(value);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var productList = new List<Product>();

            // Отримання всіх ключів з Redis
            var keys = await GetAllKeysAsync();

            // Отримання даних для кожного ключа
            foreach (var key in keys)
            {
                RedisValue value = await _redisDatabase.StringGetAsync(key);
                var product = System.Text.Json.JsonSerializer.Deserialize<Product>(value);
                productList.Add(product);
            }

            return productList;










            //IQueryable<Product> query = _dbSet;

            //var all =  await query.ToListAsync();

            //var serializedEntity = JsonSerializer.Serialize(all);

            //var cacheKey = GenerateCacheKey(query.ToString());

            //await _cache.SetStringAsync(cacheKey, serializedEntity);




            //var cachedData = await _cache.GetStringAsync(cacheKey);
            //if (!string.IsNullOrEmpty(cachedData))
            //{
            //    return JsonSerializer.Deserialize<List<Product>>(cachedData);
            //}
            //else
            //{
            //    return new List<Product>();
            //}
        }

        public async Task RemoveAsync(Product entity)
        {
            //Redis
            var cacheKey = GenerateCacheKey(entity);
            //await _cache.RemoveAsync(cacheKey);
            await _redisDatabase.KeyDeleteAsync(cacheKey);


            //БД
            //_dbSet.Remove(entity);
            //await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        private string GenerateCacheKey(Product entity)
        {
            string input = FindIdForCacheKey(entity);
            return "cache_" + input;
        }

        private string GenerateCacheKey(string input)
        {
            return "cache_" + input;
        }

        private string FindIdForCacheKey(Product entity)
        {

            return entity.Id.ToString();
        }

        private async Task<List<string>> GetAllKeysAsync()
        {
            var keys = new List<string>();

            // Використовуємо SCAN для отримання всіх ключів
            long cursor = 0;
            do
            {
                var scanResult = await _redisDatabase.ExecuteAsync("SCAN", cursor.ToString(), "MATCH", "*");
                cursor = (long)scanResult[0];

                var redisKeys = (RedisResult[])scanResult[1];
                foreach (var redisKey in redisKeys)
                {
                    keys.Add((string)redisKey);
                }
            } while (cursor != 0);

            return keys;
        }


    }
}
