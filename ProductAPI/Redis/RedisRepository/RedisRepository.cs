using ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ProductAPI.Repository.IRepository;
using ProductAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;
using System.Reflection;

namespace ProductAPI.Redis.RedisRepository
{
    public class RedisRepository<T> : IRepository<T> where T : class
    {
        private readonly IDistributedCache _cache;
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public RedisRepository(IDistributedCache cache, ApplicationDbContext db)
        {
            _cache = cache;
            _db = db;
            _dbSet = db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            //Redis
            var cacheKey = GenerateCacheKey(entity);
            var serializedData = JsonSerializer.Serialize(entity);
            await _cache.SetStringAsync(cacheKey, serializedData);

            //БД
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            var all =  await query.ToListAsync();

            var serializedEntity = JsonSerializer.Serialize(all);

            var cacheKey = GenerateCacheKey(query.ToString());

            await _cache.SetStringAsync(cacheKey, serializedEntity);




            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<T>>(cachedData);
            }
            else
            {
                return new List<T>();
            }
        }

        public async Task RemoveAsync(T entity)
        {
            //Redis
            var cacheKey = GenerateCacheKey(entity);
            await _cache.RemoveAsync(cacheKey);

            //БД
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        private string GenerateCacheKey(T entity)
        {
            string input = FindIdForCacheKey(entity);
            return "cache_" + input;
        }

        private string GenerateCacheKey(string input)
        {
            return "cache_" + input;
        }

        private string FindIdForCacheKey(T entity)
        {
            Type entityType = entity.GetType();

            PropertyInfo[] properties = entityType.GetProperties();
            string valueAsString = "";

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(Guid))
                {
                    valueAsString = Convert.ToString(property.GetValue(entity));
                }
            }
            return valueAsString;
        }
    }
}
