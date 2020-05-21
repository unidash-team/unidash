using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Unidash.Core.Domain;

namespace Unidash.Core.Infrastructure
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);

        Task<T> FindByIdAsync(string entityId);

        Task RemoveAsync(T entity);

        Task RemoveByIdAsync(string id);

        Task<T> GetOrCreateAsync(string id, T entity);

        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindAllByAsync(Expression<Func<T, bool>> match);

    }
}
