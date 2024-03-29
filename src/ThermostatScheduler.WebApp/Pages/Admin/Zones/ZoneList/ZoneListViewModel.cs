﻿using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel.Validation;
using ThermostatScheduler.WebApp.Exceptions;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneList
{
    public class ZoneListViewModel : AdminMasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        public GridViewDataSet<ZoneListListModel> HeatingZoneList { get; set; } = new();

        public ZoneListViewModel(IDependencies dependencies,
                                        IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (HeatingZoneList.IsRefreshRequired)
            {
                var heatingZones = await heatingZoneService.GetAllAsync();
                HeatingZoneList.LoadFromQueryable(heatingZones.AsQueryable());
            }

            await base.PreRender();
        }

        public async Task Delete(int id)
        {
            try
            {
                await heatingZoneService.DeleteAsync(id);
            }
            catch (EntityReferencedException)
            {
                this.AddModelError(x => x.HeatingZoneList, "Zóna nemůže být smazána, je použita v časovém programu.");
                Context.FailOnInvalidModelState();
                return;

            }

            HeatingZoneList.RequestRefresh();
        }
    }
}
