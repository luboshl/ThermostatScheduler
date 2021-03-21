using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonFlatFileDataStore;
using Microsoft.Extensions.Options;
using Scheduler.Common.Settings;
using Scheduler.Persistence.Model;

namespace Scheduler.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly DataStore store;

        private IDocumentCollection<TEntity> Collection => store.GetCollection<TEntity>();

        public Repository(IOptions<PersistenceSettings> persistenceSettings)
        {
            store = new DataStore(persistenceSettings.Value.FilePath);
        }

        public Task<ICollection<TEntity>> GetAsync(Func<TEntity, bool>? predicate = null)
        {
            var query = Collection.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            ICollection<TEntity> result = query.ToList();
            return Task.FromResult(result);
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            var result = Collection.AsQueryable()
                .SingleOrDefault(x => x.Id == id);

            if (result == null)
            {
                throw new Exception($"Item not found by ID {id}.");
            }

            return Task.FromResult(result);
        }

        public async Task CreateAsync(TEntity entity)
        {
            var succeeded = await Collection.InsertOneAsync(entity);
            if (!succeeded)
            {
                throw new Exception($"Creating of entity '{typeof(TEntity)}' failed.");
            }

            // hack - first item is created with ID=0, we want first to have ID=1
            if (entity.Id == 0)
            {
                entity.Id = 1;
                await UpdateAsync(0, entity, false);
            }
        }

        public Task UpdateAsync(int id, TEntity entity)
        {
            return UpdateAsync(id, entity, true);
        }

        public Task DeleteAsync(int id)
        {
            return Collection.DeleteOneAsync(x => x.Id == id);
        }

        private async Task UpdateAsync(int id, TEntity entity, bool updateIdFromParameter)
        {
            if (updateIdFromParameter)
            {
                entity.Id = id;
            }

            var found = await Collection.UpdateOneAsync(x => x.Id == id, entity);
            if (!found)
            {
                throw new Exception($"Item not found by ID {id}.");
            }
        }
    }
}
