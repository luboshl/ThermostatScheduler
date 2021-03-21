using System.Linq;
using System.Threading.Tasks;
using DotVVM.BusinessPack.Controls;
using DotVVM.Framework.ViewModel.Validation;
using Scheduler.App.Exceptions;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.HeatingZones.HeatingZoneList
{
    public class HeatingZoneListViewModel : MasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        public BusinessPackDataSet<HeatingZoneListModel> HeatingZoneList { get; set; } = new();

        public HeatingZoneListViewModel(IDependencies dependencies,
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
