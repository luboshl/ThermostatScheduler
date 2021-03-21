using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Common.Infrastructure
{
    public interface IInitializable
    {
        Task InitializeAsync(CancellationToken ct);
    }
}
