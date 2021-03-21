using System.Threading.Tasks;
using ThermostatScheduler.WebApp.Models;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.HeatingZones.HeatingZoneCreate
{
    public class HeatingZoneCreateViewModel : MasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        public HeatingZoneDetailModel Model { get; set; } = new();

        public HeatingZoneCreateViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public async Task Create()
        {
            await heatingZoneService.CreateAsync(Model);
            Context.RedirectToRoute(Routes.HeatingZones.HeatingZoneList);
        }
    }
}
