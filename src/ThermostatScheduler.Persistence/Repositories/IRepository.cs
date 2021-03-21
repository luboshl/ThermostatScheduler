using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduler.Persistence.Model;

namespace Scheduler.Persistence.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        Task<ICollection<TEntity>> GetAsync(Func<TEntity, bool>? predicate = null);
        Task<TEntity> GetByIdAsync(int id);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(int id, TEntity entity);
        Task DeleteAsync(int id);
    }
}
