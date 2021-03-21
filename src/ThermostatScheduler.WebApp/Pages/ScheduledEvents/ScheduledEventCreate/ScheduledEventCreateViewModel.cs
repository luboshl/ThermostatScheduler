using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.ScheduledEvents.ScheduledEventCreate
{
    public class ScheduledEventCreateViewModel : MasterPageViewModel
    {
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        public ScheduledEventDetailModel Model { get; set; } = null!;
        public ICollection<HeatingZoneListModel> HeatingZones { get; set; } = null!;

        public ScheduledEventCreateViewModel(IDependencies dependencies,
                                             IScheduledEventService scheduledEventService,
                                             IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.scheduledEventService = scheduledEventService;
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            Model = new ScheduledEventDetailModel
            {
                Time = DateTime.Today.AddHours(12),
                Temperature = 20
            };
            HeatingZones = await heatingZoneService.GetAllAsync();
            await base.PreRender();
        }

        public async Task Create()
        {
            await scheduledEventService.CreateAsync(Model);
            Context.RedirectToRoute(Routes.ScheduledEvents.ScheduledEventList);
        }
    }
}
