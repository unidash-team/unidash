using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unidash.Core.Domain;

namespace Unidash.Core.Infrastructure
{
    public class InMemoryEntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        private List<T> _list = new List<T>();

        public async Task<T> AddAsync(T entity)
        {
            _list.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<T> FindAsync(string entityId)
        {
            var entity = _list.SingleOrDefault(e => e.Id == entityId);
            return await Task.FromResult(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            var e = await FindAsync(entity.Id);
            _list.Remove(e);
        }

        public Task RemoveAsync(string id)
        {
            _list.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }

        public async Task<T> GetOrCreateAsync(string id, T entity)
        {
            var inlineEntity = await FindAsync(id);

            if (inlineEntity != null) return inlineEntity;

            // Make sure we apply the ID
            inlineEntity = entity;
            inlineEntity.Id = id;
            inlineEntity = await AddAsync(inlineEntity);

            return inlineEntity;
        }

        public Task<IEnumerable<T>> FindAllAsync() => Task.FromResult(_list.ToList() as IEnumerable<T>);

        public Task<IEnumerable<T>> FindByPredicateAsync(Expression<Func<T, bool>> match) =>
            Task.FromResult(_list.Where(match.Compile()));

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> AsQueryable() => _list.AsQueryable();
    }
}
