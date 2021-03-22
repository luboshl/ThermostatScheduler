using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.Persistence.Model;

namespace ThermostatScheduler.Persistence.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        Task<ICollection<TEntity>> GetAsync(Func<TEntity, bool>? predicate = null);
        Task<TEntity> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task UpdateAsync(int id, TEntity entity);
        Task DeleteAsync(int id);
    }
}
