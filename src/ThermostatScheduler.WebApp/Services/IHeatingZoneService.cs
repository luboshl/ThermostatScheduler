using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduler.App.Models;

namespace Scheduler.App.Services
{
    public interface IHeatingZoneService
    {
        Task<ICollection<HeatingZoneListModel>> GetAllAsync();
        Task<HeatingZoneDetailModel> GetByIdAsync(int id);
        Task CreateAsync(HeatingZoneDetailModel model);
        Task UpdateAsync(HeatingZoneDetailModel model);
        Task DeleteAsync(int id);
    }
}
