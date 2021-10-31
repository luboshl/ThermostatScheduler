using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Pages.Admin.Events.EventDetail;
using ThermostatScheduler.WebApp.Pages.Admin.Events.EventList;

namespace ThermostatScheduler.WebApp.Services
{
    public interface IScheduledEventService
    {
        Task<ICollection<EventListModel>> GetAllAsync();
        Task<EventDetailModel> GetByIdAsync(int id);
        Task CreateAsync(EventDetailModel model);
        Task UpdateAsync(EventDetailModel model);
        Task DeleteAsync(int id);
        Task<int> CloneAsync(int id);
    }
}
