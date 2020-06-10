using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unidash.Core.Domain;

namespace Unidash.Core.Infrastructure
{
    public class MongoDbConnectionOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

    public class MongoEntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        public string ConnectionString { get; }

        public string DatabaseName { get; }

        private readonly MongoClient _client;

        private readonly IMongoDatabase _database;

        public MongoEntityRepository(IOptions<MongoDbConnectionOptions> options)
            : this(options.Value.ConnectionString, options.Value.DatabaseName)
        {
        }

        public MongoEntityRepository(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;

            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase(DatabaseName);
        }

        public async Task<T> AddAsync(T entity)
        {
            await EntityCollection.InsertOneAsync(entity);
            return await FindByIdAsync(entity.Id);
        }

        public async Task<T> FindByIdAsync(string entityId)
        {
            return await EntityCollection
                .Find(e => e.Id == entityId)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            await EntityCollection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task RemoveByIdAsync(string id)
        {
            await EntityCollection.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<T> GetOrCreateAsync(string id, T entity)
        {
            var match = await EntityCollection
                .Find(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (match != null) return match;

            entity.Id = id;
            return await AddAsync(entity);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            var entities = await EntityCollection.FindAsync(x => x.Id != string.Empty);
            return await entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllByAsync(Expression<Func<T, bool>> match)
        {
            var entities = await EntityCollection
                .AsQueryable()
                .Where(match)
                .ToListAsync(); // Hackerman

            return entities;
        }

        private IMongoCollection<T> EntityCollection => _database.GetCollection<T>($"{typeof(T).Namespace}.{typeof(T).Name}");
    }
}
