using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.ScheduledEvents.ScheduledEventEdit
{
    public class ScheduledEventEditViewModel : MasterPageViewModel
    {
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Id")]
        public int Id { get; set; }

        public ScheduledEventDetailModel Model { get; set; } = null!;
        public ICollection<HeatingZoneListModel> HeatingZones { get; set; } = null!;

        public ScheduledEventEditViewModel(IDependencies dependencies,
                                           IScheduledEventService scheduledEventService,
                                           IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.scheduledEventService = scheduledEventService;
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            Model = await scheduledEventService.GetByIdAsync(Id);
            HeatingZones = await heatingZoneService.GetAllAsync();
            await base.PreRender();
        }

        public async Task Save()
        {
            await scheduledEventService.UpdateAsync(Model);
            Context.RedirectToRoute(Routes.ScheduledEvents.ScheduledEventList);
        }
    }
}
