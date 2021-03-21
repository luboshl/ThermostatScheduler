using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Processing
{
    public interface ISchedulerManager
    {
        Task RestartAsync(CancellationToken ct);
    }
}
