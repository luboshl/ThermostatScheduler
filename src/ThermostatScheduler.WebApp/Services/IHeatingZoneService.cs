using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneDetail;
using ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneList;

namespace ThermostatScheduler.WebApp.Services
{
    public interface IHeatingZoneService
    {
        Task<ICollection<ZoneListListModel>> GetAllAsync();
        Task<ZoneDetailModel> GetByIdAsync(int id);
        Task<ZoneDetailModel> GetNameByCodeAsync(string code);
        Task CreateAsync(string name, string code);
        Task UpdateAsync(int id, string name, string code);
        Task DeleteAsync(int id);
    }
}
