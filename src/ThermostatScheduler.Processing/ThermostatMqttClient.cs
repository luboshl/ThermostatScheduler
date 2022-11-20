using System;
using System.Threading.Tasks;
using ThermostatScheduler.Common.Infrastructure;
using ThermostatScheduler.Common.Infrastructure.Mqtt;

namespace ThermostatScheduler.Processing
{
    public class ThermostatMqttClient : IThermostatClient
    {
        private readonly IMqttPublisher mqttPublisher;
        private readonly IDateTimeProvider dateTimeProvider;

        public ThermostatMqttClient(IMqttPublisher mqttPublisher,
                                    IDateTimeProvider dateTimeProvider)
        {
            this.mqttPublisher = mqttPublisher;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Task SetTemperatureAsync(int scheduledEventId, string heatingZoneCode, string? heatingZoneName, double temperature)
        {
            var payload = new
            {
                scheduledEventId,
                heatingZoneCode,
                heatingZoneName,
                temperature,
                timestamp = dateTimeProvider.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var topic = $"thermostat/{heatingZoneCode}/set-temperature";
            return mqttPublisher.PublishAsync(topic, payload.ToJsonString());
        }
    }
}
