using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThermostatScheduler.Persistence.Model;
using ThermostatScheduler.Persistence.Repositories;
using ThermostatScheduler.WebApp.Exceptions;
using ThermostatScheduler.WebApp.Models;

namespace ThermostatScheduler.WebApp.Services
{
    public class HeatingZoneService : IHeatingZoneService
    {
        private readonly IRepository<HeatingZone> heatingZoneRepository;
        private readonly IRepository<ScheduledEvent> scheduledEventRepository;

        public HeatingZoneService(IRepository<HeatingZone> heatingZoneRepository,
                                  IRepository<ScheduledEvent> scheduledEventRepository)
        {
            this.heatingZoneRepository = heatingZoneRepository;
            this.scheduledEventRepository = scheduledEventRepository;
        }

        public async Task<ICollection<HeatingZoneListModel>> GetAllAsync()
        {
            var entities = await heatingZoneRepository.GetAsync();
            return entities.Select(x => new HeatingZoneListModel(x.Id, x.Name, x.Code)).ToList();
        }

        public async Task<HeatingZoneDetailModel> GetByIdAsync(int id)
        {
            var entity = await heatingZoneRepository.GetByIdAsync(id);
            return new HeatingZoneDetailModel(entity.Id, entity.Name, entity.Code);
        }

        public async Task CreateAsync(HeatingZoneDetailModel model)
        {
            var entity = new HeatingZone(model.Name, model.Code);
            await heatingZoneRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(HeatingZoneDetailModel model)
        {
            var entity = new HeatingZone(model.Name, model.Code);
            await heatingZoneRepository.UpdateAsync(model.Id, entity);
        }

        public async Task DeleteAsync(int id)
        {
            var scheduledEvents = await scheduledEventRepository.GetAsync(x => x.HeatingZoneId == id);
            if (scheduledEvents.Any())
            {
                throw new EntityReferencedException();
            }

            await heatingZoneRepository.DeleteAsync(id);
        }
    }
}
