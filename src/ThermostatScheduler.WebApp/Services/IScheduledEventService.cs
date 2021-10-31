using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Pages.ScheduledEvents.ScheduledEventDetail;
using ThermostatScheduler.WebApp.Pages.ScheduledEvents.ScheduledEventList;

namespace ThermostatScheduler.WebApp.Services
{
    public interface IScheduledEventService
    {
        Task<ICollection<ScheduledEventListModel>> GetAllAsync();
        Task<ScheduledEventDetailModel> GetByIdAsync(int id);
        Task CreateAsync(ScheduledEventDetailModel model);
        Task UpdateAsync(ScheduledEventDetailModel model);
        Task DeleteAsync(int id);
        Task<int> CloneAsync(int id);
    }
}
