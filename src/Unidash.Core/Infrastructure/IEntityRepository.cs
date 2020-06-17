using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unidash.Core.Domain;

namespace Unidash.Core.Infrastructure
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);

        Task<T> FindAsync(string entityId);

        Task RemoveAsync(T entity);

        Task RemoveAsync(string id);

        Task<T> GetOrCreateAsync(string id, T entity);

        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindByPredicateAsync(Expression<Func<T, bool>> match);

        Task UpdateAsync(T entity);

        IQueryable<T> AsQueryable();
    }
}
