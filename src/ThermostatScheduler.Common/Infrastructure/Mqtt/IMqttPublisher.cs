using System.Threading.Tasks;

namespace Scheduler.Common.Infrastructure.Mqtt
{
    public interface IMqttPublisher
    {
        Task PublishAsync(string topic, string? payload = null);
    }
}
