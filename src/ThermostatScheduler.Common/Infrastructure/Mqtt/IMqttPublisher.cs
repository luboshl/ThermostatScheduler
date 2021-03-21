using System.Threading.Tasks;

namespace ThermostatScheduler.Common.Infrastructure.Mqtt
{
    public interface IMqttPublisher
    {
        Task PublishAsync(string topic, string? payload = null);
    }
}
