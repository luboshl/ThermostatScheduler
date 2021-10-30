using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ThermostatScheduler.Common;
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
        private readonly ICurrentScheduleCalculator currentScheduleCalculator;

        public ScheduledEventService(ILogger<ScheduledEventService> logger,
                                     IRepository<ScheduledEvent> scheduledEventRepository,
                                     IRepository<HeatingZone> heatingZoneRepository,
                                     IDateTimeProvider dateTimeProvider,
                                     ISchedulerManager schedulerManager,
                                     ICurrentScheduleCalculator currentScheduleCalculator)
        {
            this.logger = logger;
            this.scheduledEventRepository = scheduledEventRepository;
            this.heatingZoneRepository = heatingZoneRepository;
            this.dateTimeProvider = dateTimeProvider;
            this.schedulerManager = schedulerManager;
            this.currentScheduleCalculator = currentScheduleCalculator;
        }

        public async Task<ICollection<ScheduledEventListModel>> GetAllAsync()
        {
            var heatingZones = await heatingZoneRepository.GetAsync();
            var heatingZonesById = heatingZones.ToDictionary(x => x.Id);
            var scheduledEvent = await scheduledEventRepository.GetAsync();
            var currentSchedules = currentScheduleCalculator.GetCurrentSchedules(scheduledEvent);

            return scheduledEvent.Select(x =>
                    new ScheduledEventListModel(
                        x.Id,
                        x.HeatingZoneId,
                        heatingZonesById[x.HeatingZoneId].Name,
                        GetDateTime(x.Time),
                        x.Temperature,
                        x.Mode,
                        x.ValidFrom,
                        x.ValidTo,
                        currentSchedules.Contains(x)))
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
                scheduledEvent.Temperature,
                scheduledEvent.ValidFrom,
                scheduledEvent.ValidTo,
                scheduledEvent.Mode);
        }

        public async Task CreateAsync(ScheduledEventDetailModel model)
        {
            var entity = MapScheduledEvent(model);
            await ValidateHeatingZone(entity.HeatingZoneId);

            await scheduledEventRepository.CreateAsync(entity);
            RestartScheduler();
        }

        public async Task UpdateAsync(ScheduledEventDetailModel model)
        {
            var entity = MapScheduledEvent(model);
            await ValidateHeatingZone(entity.HeatingZoneId);

            await scheduledEventRepository.UpdateAsync(model.Id, entity);
            RestartScheduler();
        }

        public async Task DeleteAsync(int id)
        {
            await scheduledEventRepository.DeleteAsync(id);
            RestartScheduler();
        }

        public async Task<int> CloneAsync(int id)
        {
            var original = await scheduledEventRepository.GetByIdAsync(id);
            var clone = original.GetClone();
            return await scheduledEventRepository.CreateAsync(clone);
        }

        private static ScheduledEvent MapScheduledEvent(ScheduledEventDetailModel model)
        {
            var validTo = model.SelectedScheduleMode == ScheduleMode.OneTimeOnly
                ? model.ValidFrom ?? throw new InvalidOperationException($"{nameof(model.ValidFrom)} not set for {nameof(ScheduleMode.OneTimeOnly)} mode.")
                : model.ValidTo;
            
            var entity = new ScheduledEvent(
                model.HeatingZoneId,
                model.Time.TimeOfDay,
                model.Temperature,
                model.ValidFrom,
                validTo,
                model.SelectedScheduleMode);
            return entity;
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

        private async Task ValidateHeatingZone(int heatingZoneId)
        {
            var heatingZones = await heatingZoneRepository.GetAsync(x => x.Id == heatingZoneId);
            if (heatingZones.Count == 0)
            {
                throw new Exception($"Heating zone not found by ID {heatingZoneId}.");
            }
        }
    }
}
