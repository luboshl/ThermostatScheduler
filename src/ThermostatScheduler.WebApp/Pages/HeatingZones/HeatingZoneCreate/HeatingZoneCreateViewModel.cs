using System.Threading.Tasks;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.HeatingZones.HeatingZoneCreate
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
