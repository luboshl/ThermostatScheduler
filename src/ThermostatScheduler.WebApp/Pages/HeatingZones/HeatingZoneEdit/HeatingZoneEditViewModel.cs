using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.HeatingZones.HeatingZoneEdit
{
    public class HeatingZoneEditViewModel : MasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Id")]
        public int Id { get; set; }

        public HeatingZoneDetailModel Model { get; set; } = new();

        public HeatingZoneEditViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            Model = await heatingZoneService.GetByIdAsync(Id);
            await base.PreRender();
        }

        public async Task Save()
        {
            await heatingZoneService.UpdateAsync(Model);
            Context.RedirectToRoute(Routes.HeatingZones.HeatingZoneList);
        }
    }
}
