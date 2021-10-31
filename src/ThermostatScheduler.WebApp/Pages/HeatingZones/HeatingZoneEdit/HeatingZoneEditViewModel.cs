﻿using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.WebApp.Models;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.HeatingZones.HeatingZoneEdit
{
    public class HeatingZoneEditViewModel : MasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Id")]
        public int Id { get; set; }

        public bool IsEditMode => Id != 0;

        public HeatingZoneDetailModel Model { get; set; } = null!;

        public HeatingZoneEditViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (IsEditMode)
            {
                Model = await heatingZoneService.GetByIdAsync(Id);
            }
            else
            {
                Model = new HeatingZoneDetailModel();
            }

            await base.PreRender();
        }

        public async Task Save()
        {
            if (IsEditMode)
            {
                await heatingZoneService.UpdateAsync(Model);
            }
            else
            {
                await heatingZoneService.CreateAsync(Model);
            }

            Context.RedirectToRoute(Routes.HeatingZones.HeatingZoneList);
        }
    }
}
