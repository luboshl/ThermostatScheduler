using System.Threading.Tasks;

namespace ThermostatScheduler.Processing
{
    public interface IThermostatClient
    {
        Task SetTemperatureAsync(int scheduledEventId, string heatingZoneCode, string? heatingZoneName, double temperature);
    }
}
