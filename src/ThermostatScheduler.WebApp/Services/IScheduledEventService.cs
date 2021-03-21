using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Models;

namespace ThermostatScheduler.WebApp.Services
{
    public interface IScheduledEventService
    {
        Task<ICollection<ScheduledEventListModel>> GetAllAsync();
        Task<ScheduledEventDetailModel> GetByIdAsync(int id);
        Task CreateAsync(ScheduledEventDetailModel model);
        Task UpdateAsync(ScheduledEventDetailModel model);
        Task DeleteAsync(int id);
    }
}
