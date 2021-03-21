using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace ThermostatScheduler.Common.Infrastructure.Mqtt
{
    public class MqttPublisher : IMqttPublisher
    {
        private readonly ILogger<MqttPublisher> logger;
        private readonly IMqttClientAdapter mqttClientAdapter;

        public MqttPublisher(ILogger<MqttPublisher> logger,
                             IMqttClientAdapter mqttClientAdapter)
        {
            this.logger = logger;
            this.mqttClientAdapter = mqttClientAdapter;
        }

        public async Task PublishAsync(string topic, string? payload = null)
        {
            logger.LogDebug("Publish message via MQTT (Topic: {topic}, Payload: {payload}).", topic, payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            if (mqttClientAdapter.Client.IsConnected)
            {
                try
                {
                    await mqttClientAdapter.Client.PublishAsync(message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Publishing to MQTT failed.", ex);
                    ;
                }
            }
            else
            {
                logger.LogWarning("MQTT client is not connected, message not published via MQTT (Topic: {topic}, Payload: {payload}).", topic, payload);
            }
        }
    }
}
