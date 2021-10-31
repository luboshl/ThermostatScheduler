using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using ThermostatScheduler.Common;
using ThermostatScheduler.Common.Infrastructure;
using ThermostatScheduler.Persistence.Model;
using ThermostatScheduler.Persistence.Repositories;

namespace ThermostatScheduler.Processing
{
    public class SchedulerManager : ISchedulerManager, IHostedService
    {
        private IScheduler? quartzScheduler;

        private readonly ILogger<SchedulerManager> logger;
        private readonly IRepository<ScheduledEvent> scheduledEventRepository;
        private readonly IRepository<HeatingZone> heatingZoneRepository;
        private readonly IServiceProvider serviceProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public SchedulerManager(ILogger<SchedulerManager> logger,
                                IRepository<ScheduledEvent> scheduledEventRepository,
                                IRepository<HeatingZone> heatingZoneRepository,
                                IServiceProvider serviceProvider,
                                IDateTimeProvider dateTimeProvider)
        {
            this.logger = logger;
            this.scheduledEventRepository = scheduledEventRepository;
            this.heatingZoneRepository = heatingZoneRepository;
            this.serviceProvider = serviceProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            logger.LogInformation("Starting.");
            await StartQuartzAsync(ct);
            logger.LogInformation("Started.");
        }

        public async Task StopAsync(CancellationToken ct)
        {
            logger.LogInformation("Stopping.");
            await StopQuartzAsync(ct);
            logger.LogInformation("Stopped.");
        }

        public async Task RestartAsync(CancellationToken ct)
        {
            logger.LogInformation("Restarting.");
            await StopAsync(ct);
            await Task.Delay(500, ct);
            await StartAsync(ct);
            logger.LogInformation("Restarted.");
        }

        private async Task StartQuartzAsync(CancellationToken ct)
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            quartzScheduler = await factory.GetScheduler(ct);
            quartzScheduler.JobFactory = new ServiceProviderJobFactory(serviceProvider);
            await ScheduleEvents(quartzScheduler, ct);
            await quartzScheduler.Start(ct);
        }

        private async Task StopQuartzAsync(CancellationToken ct)
        {
            if (quartzScheduler != null)
            {
                await quartzScheduler.Shutdown(ct);
                quartzScheduler = null;
            }
        }

        private async Task ScheduleEvents(IScheduler scheduler, CancellationToken ct)
        {
            var scheduledEvents = await scheduledEventRepository.GetAsync();
            var heatingZones = await heatingZoneRepository.GetAsync();
            var heatingZonesById = heatingZones.ToDictionary(x => x.Id);

            scheduledEvents = scheduledEvents
                .Where(x => x.ValidFrom == null || x.ValidFrom.Value <= dateTimeProvider.Now)
                .Where(x => x.ValidTo == null || dateTimeProvider.Now < x.ValidTo.Value.AddDays(1))
                .ToList();

            foreach (var scheduledEvent in scheduledEvents)
            {
                await ConfigureQuartzJob(scheduler, scheduledEvent, heatingZonesById[scheduledEvent.HeatingZoneId], ct);
            }
        }

        private async Task ConfigureQuartzJob(IScheduler scheduler, ScheduledEvent scheduledEvent, HeatingZone heatingZone, CancellationToken ct)
        {
            var job = JobBuilder.Create<SetTemperatureJob>()
                .WithIdentity($"id-{scheduledEvent.Id}")
                .UsingJobData(SetTemperatureJob.Key.ScheduledEventId, scheduledEvent.Id)
                .UsingJobData(SetTemperatureJob.Key.HeatingZoneCode, heatingZone.Code)
                .UsingJobData(SetTemperatureJob.Key.HeatingZoneName, heatingZone.Name)
                .UsingJobData(SetTemperatureJob.Key.Temperature, scheduledEvent.Temperature)
                .Build();

            var startDate = GetStartDateTime(scheduledEvent);
            var startDateTimeLocal = startDate.Add(scheduledEvent.Time);
            var startDateTimeUtc = startDateTimeLocal.ToUniversalTime();

            var triggerBuilder = TriggerBuilder.Create();
            triggerBuilder = triggerBuilder.StartAt(new DateTimeOffset(startDateTimeUtc));

            var logMessage = $"Event ID {scheduledEvent.Id} scheduled to {startDateTimeLocal} (UTC {startDateTimeUtc})";

            switch (scheduledEvent.Mode)
            {
                case ScheduleMode.RepeatDaily:
                {
                    var oneDayInHours = 24;
                    triggerBuilder = triggerBuilder
                        .WithSimpleSchedule(builder => builder
                            .WithMisfireHandlingInstructionNextWithExistingCount()
                            .WithIntervalInHours(oneDayInHours)
                            .RepeatForever());

                    logMessage += $" with interval in {oneDayInHours} hours";
                    break;
                }
                case ScheduleMode.OneTimeOnly:
                    triggerBuilder = triggerBuilder
                        .WithSimpleSchedule(builder => builder
                            .WithMisfireHandlingInstructionNextWithExistingCount());
                    logMessage += " as one time only event";
                    break;
                default:
                    logger.LogError($"Event ID {scheduledEvent.Id} - unknown {nameof(ScheduleMode)}: {scheduledEvent.Mode}.");
                    return;
            }

            if (scheduledEvent.ValidTo is not null)
            {
                var endDate = scheduledEvent.ValidTo.Value.Date;
                var endDateTimeLocal = endDate.Add(scheduledEvent.Time);
                var endDateTimeUtc = endDateTimeLocal.ToUniversalTime();
                triggerBuilder.EndAt(new DateTimeOffset(endDateTimeUtc));

                logMessage += $". Valid to {endDateTimeLocal} (UTC {endDateTimeUtc})";
            }

            var trigger = triggerBuilder.Build();

            await scheduler.ScheduleJob(job, trigger, ct);
            logger.LogInformation(logMessage + ".");
        }

        private DateTime GetStartDateTime(ScheduledEvent scheduledEvent)
        {
            if (scheduledEvent.ValidFrom is null)
            {
                return dateTimeProvider.Now.Date;
            }

            if (scheduledEvent.ValidFrom.Value.Date < dateTimeProvider.Now.Date)
            {
                return dateTimeProvider.Now.Date;
            }

            return scheduledEvent.ValidFrom.Value.Date;
        }
    }
}
