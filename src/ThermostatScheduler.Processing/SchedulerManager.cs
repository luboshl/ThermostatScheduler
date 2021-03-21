using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
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

        public SchedulerManager(ILogger<SchedulerManager> logger,
                                IRepository<ScheduledEvent> scheduledEventRepository,
                                IRepository<HeatingZone> heatingZoneRepository,
                                IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.scheduledEventRepository = scheduledEventRepository;
            this.heatingZoneRepository = heatingZoneRepository;
            this.serviceProvider = serviceProvider;
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

            var dateTimeLocal = DateTime.Today.Add(scheduledEvent.Time);
            var dateTimeUtc = dateTimeLocal.ToUniversalTime();
            var hourInterval = 24;

            var trigger = TriggerBuilder.Create()
                .StartAt(new DateTimeOffset(dateTimeUtc))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(hourInterval)
                    .RepeatForever())
                .Build();

            logger.LogInformation("Event ID {id} scheduled at {time} (UTC {dateTimeUtc}) with interval in {interval} hours.",
                scheduledEvent.Id, scheduledEvent.Time, dateTimeUtc, hourInterval);
            
            await scheduler.ScheduleJob(job, trigger, ct);
        }
    }
}
