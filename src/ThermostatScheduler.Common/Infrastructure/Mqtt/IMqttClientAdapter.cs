using System;
using System.Threading.Tasks;
using MQTTnet.Client;

namespace ThermostatScheduler.Common.Infrastructure.Mqtt
{
    public interface IMqttClientAdapter
    {
        IMqttClient Client { get; }

        event Func<Task>? Connected;
        event Func<Task>? Disconnected;
    }
}
