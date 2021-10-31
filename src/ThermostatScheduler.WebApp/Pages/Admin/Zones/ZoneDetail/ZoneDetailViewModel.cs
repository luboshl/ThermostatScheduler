using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneDetail
{
    public class ZoneDetailViewModel : AdminMasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Id")]
        public int Id { get; set; }

        public bool IsEditMode => Id != 0;

        public ZoneDetailModel Model { get; set; } = null!;

        public ZoneDetailViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
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
                Model = new ZoneDetailModel();
            }

            await base.PreRender();
        }

        public async Task Save()
        {
            if (IsEditMode)
            {
                await heatingZoneService.UpdateAsync(Model.Id, Model.Name, Model.Code);
            }
            else
            {
                await heatingZoneService.CreateAsync(Model.Name, Model.Code);
            }

            Context.RedirectToRoute(Routes.Admin.Zones.ZoneList);
        }
    }
}
