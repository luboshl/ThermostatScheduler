using System.Threading;
using System.Threading.Tasks;

namespace ThermostatScheduler.Common.Infrastructure
{
    public interface IInitializable
    {
        Task InitializeAsync(CancellationToken ct);
    }
}
