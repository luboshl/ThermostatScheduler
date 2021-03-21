using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ThermostatScheduler.Common.Infrastructure;
using ThermostatScheduler.Persistence.Model;
using ThermostatScheduler.Persistence.Repositories;
using ThermostatScheduler.Processing;
using ThermostatScheduler.WebApp.Models;

namespace ThermostatScheduler.WebApp.Services
{
    public class ScheduledEventService : IScheduledEventService
    {
        private readonly ILogger<ScheduledEventService> logger;
        private readonly IRepository<ScheduledEvent> scheduledEventRepository;
        private readonly IRepository<HeatingZone> heatingZoneRepository;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISchedulerManager schedulerManager;

        public ScheduledEventService(ILogger<ScheduledEventService> logger,
                                     IRepository<ScheduledEvent> scheduledEventRepository,
                                     IRepository<HeatingZone> heatingZoneRepository,
                                     IDateTimeProvider dateTimeProvider,
                                     ISchedulerManager schedulerManager)
        {
            this.logger = logger;
            this.scheduledEventRepository = scheduledEventRepository;
            this.heatingZoneRepository = heatingZoneRepository;
            this.dateTimeProvider = dateTimeProvider;
            this.schedulerManager = schedulerManager;
        }

        public async Task<ICollection<ScheduledEventListModel>> GetAllAsync()
        {
            var heatingZones = await heatingZoneRepository.GetAsync();
            var heatingZonesById = heatingZones.ToDictionary(x => x.Id);
            var scheduledEvent = await scheduledEventRepository.GetAsync();

            return scheduledEvent.Select(x =>
                    new ScheduledEventListModel(
                        x.Id,
                        x.HeatingZoneId,
                        heatingZonesById[x.HeatingZoneId].Name,
                        GetDateTime(x.Time),
                        x.Temperature,
                        x.Note))
                .ToList();
        }

        public async Task<ScheduledEventDetailModel> GetByIdAsync(int id)
        {
            var scheduledEvent = await scheduledEventRepository.GetByIdAsync(id);
            var heatingZone = await heatingZoneRepository.GetByIdAsync(scheduledEvent.HeatingZoneId);

            return new ScheduledEventDetailModel(
                scheduledEvent.Id,
                heatingZone.Id,
                heatingZone.Name,
                GetDateTime(scheduledEvent.Time),
                (double)scheduledEvent.Temperature,
                scheduledEvent.Note);
        }

        public async Task CreateAsync(ScheduledEventDetailModel model)
        {
            var entity = new ScheduledEvent(model.HeatingZoneId, model.Time.TimeOfDay, model.Temperature, model.Note);
            await scheduledEventRepository.CreateAsync(entity);
            RestartScheduler();
        }

        public async Task UpdateAsync(ScheduledEventDetailModel model)
        {
            var entity = new ScheduledEvent(model.HeatingZoneId, model.Time.TimeOfDay, model.Temperature, model.Note);
            await scheduledEventRepository.UpdateAsync(model.Id, entity);
            RestartScheduler();
        }

        public async Task DeleteAsync(int id)
        {
            await scheduledEventRepository.DeleteAsync(id);
            RestartScheduler();
        }

        private DateTime GetDateTime(TimeSpan time)
        {
            return dateTimeProvider.Now.Date.Add(time);
        }

        private void RestartScheduler()
        {
            Task.Run(() =>
            {
                try
                {
                    schedulerManager.RestartAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Restarting of scheduler failed.");
                }
            });
        }
    }
}
