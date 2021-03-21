using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Processing
{
    public class SetTemperatureJob : IJob
    {
        private readonly ILogger<SetTemperatureJob> logger;
        private readonly IThermostatClient thermostatClient;

        public SetTemperatureJob(ILogger<SetTemperatureJob> logger,
                                 IThermostatClient thermostatClient)
        {
            this.logger = logger;
            this.thermostatClient = thermostatClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.LogInformation("Executing job.");
                var dataMap = context.JobDetail.JobDataMap;
                var scheduledEventId = dataMap.GetIntValue(Key.ScheduledEventId);
                var heatingZoneCode = dataMap.GetString(Key.HeatingZoneCode);
                var heatingZoneName = dataMap.GetString(Key.HeatingZoneName);
                var temperature = dataMap.GetDoubleValue(Key.Temperature);

                if (String.IsNullOrWhiteSpace(heatingZoneCode))
                {
                    logger.LogError("Missing value for {key}.", Key.HeatingZoneCode);
                    return;
                }

                if (temperature == 0)
                {
                    logger.LogError("Missing value for {key}.", Key.Temperature);
                    return;
                }

                await thermostatClient.SetTemperatureAsync(scheduledEventId, heatingZoneCode, heatingZoneName, temperature);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Job failed.");
                throw new JobExecutionException(ex);
            }
        }

        public static class Key
        {
            public const string ScheduledEventId = nameof(ScheduledEventId);
            public const string HeatingZoneCode = nameof(HeatingZoneCode);
            public const string HeatingZoneName = nameof(HeatingZoneName);
            public const string Temperature = nameof(Temperature);
        }
    }
}
