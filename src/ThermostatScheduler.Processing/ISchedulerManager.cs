using System.Threading;
using System.Threading.Tasks;

namespace ThermostatScheduler.Processing
{
    public interface ISchedulerManager
    {
        Task RestartAsync(CancellationToken ct);
    }
}
