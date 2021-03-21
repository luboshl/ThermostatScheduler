using System;
using System.Threading.Tasks;
using ThermostatScheduler.Common.Infrastructure.Mqtt;

namespace ThermostatScheduler.Processing
{
    public class ThermostatMqttClient : IThermostatClient
    {
        private readonly IMqttPublisher mqttPublisher;

        public ThermostatMqttClient(IMqttPublisher mqttPublisher)
        {
            this.mqttPublisher = mqttPublisher;
        }

        public Task SetTemperatureAsync(int scheduledEventId, string heatingZoneCode, string? heatingZoneName, double temperature)
        {
            var payload = new
            {
                scheduledEventId,
                heatingZoneCode,
                heatingZoneName,
                temperature
            };

            string topic = $"thermostat/{heatingZoneCode}/set-temperature";
            return mqttPublisher.PublishAsync(topic, payload.ToJsonString());
        }
    }
}
