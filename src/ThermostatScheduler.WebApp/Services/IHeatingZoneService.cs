﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Pages.Admin.HeatingZones.HeatingZoneDetail;
using ThermostatScheduler.WebApp.Pages.Admin.HeatingZones.HeatingZoneList;

namespace ThermostatScheduler.WebApp.Services
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
