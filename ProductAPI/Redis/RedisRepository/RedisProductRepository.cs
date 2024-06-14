using AutoMapper;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Reflection;

namespace ProductAPI.Redis.RedisRepository
{
    public class RedisProductRepository : RedisRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IDistributedCache _cache;
        public RedisProductRepository(IDistributedCache cache, ApplicationDbContext db) : base(cache, db)
        {
            _cache = cache;
            _db = db;
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            var serializedEntity = JsonSerializer.Serialize(entity);

            var cacheKey = GenerateCacheKey(entity);

            await _cache.SetStringAsync(cacheKey, serializedEntity);



            entity.UpdatedDate = DateTime.Now;
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        private string GenerateCacheKey(Product entity)
        {
            string input = entity.Id.ToString();
            return "cache_" + input;
        }
    }
}
