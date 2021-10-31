using System;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.HomeAssistant.Zones.TemporaryTemperature
{
    public class TemporaryTemperatureViewModel : HomeAssistantMasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Code")]
        public string Code { get; set; } = null!;

        public bool ShowSuccessAlert { get; set; }

        public TemporaryTemperatureModel Model { get; set; } = null!;

        public TemporaryTemperatureViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                throw new InvalidOperationException($"{Code} cannot be empty.");
            }

            var zone = await heatingZoneService.GetNameByCodeAsync(Code);
            Model = new TemporaryTemperatureModel(zone.Id, zone.Name, zone.Code);
            await base.PreRender();
        }

        public async Task Save()
        {
            await heatingZoneService.UpdateAsync(Model.Id, Model.Name, Model.Code);
            ShowSuccessAlert = true;
        }
    }
}
