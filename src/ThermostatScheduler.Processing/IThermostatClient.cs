using System.Threading.Tasks;

namespace Scheduler.Processing
{
    public interface IThermostatClient
    {
        Task SetTemperatureAsync(int scheduledEventId, string heatingZoneCode, string? heatingZoneName, double temperature);
    }
}
