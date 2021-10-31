using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Pages.Admin.HeatingZones.HeatingZoneDetail;
using ThermostatScheduler.WebApp.Pages.Admin.HeatingZones.HeatingZoneList;

namespace ThermostatScheduler.WebApp.Services
{
    public interface IHeatingZoneService
    {
        Task<ICollection<HeatingZoneListModel>> GetAllAsync();
        Task<HeatingZoneDetailModel> GetByIdAsync(int id);
        Task<HeatingZoneDetailModel> GetNameByCodeAsync(string code);
        Task CreateAsync(string name, string code);
        Task UpdateAsync(int id, string name, string code);
        Task DeleteAsync(int id);
    }
}
