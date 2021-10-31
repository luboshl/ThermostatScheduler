using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.Common;
using ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneList;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.Admin.Events.EventDetail
{
    public class EventDetailViewModel : AdminMasterPageViewModel
    {
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Id")]
        public int Id { get; set; }

        public bool IsEditMode => Id != 0;

        public EventDetailModel Model { get; set; } = null!;
        public ICollection<ZoneListListModel> HeatingZones { get; set; } = null!;

        public EventDetailViewModel(IDependencies dependencies,
                                           IScheduledEventService scheduledEventService,
                                           IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.scheduledEventService = scheduledEventService;
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (IsEditMode)
            {
                Model = await scheduledEventService.GetByIdAsync(Id);
            }
            else
            {
                Model = new EventDetailModel
                {
                    Time = DateTime.Today.AddHours(12),
                    Temperature = 20,
                    SelectedScheduleMode = ScheduleMode.RepeatDaily
                };
            }

            HeatingZones = await heatingZoneService.GetAllAsync();
            await base.PreRender();
        }

        public async Task Save()
        {
            if (IsEditMode)
            {
                await scheduledEventService.UpdateAsync(Model);
            }
            else
            {
                await scheduledEventService.CreateAsync(Model);
            }

            Context.RedirectToRoute(Routes.Admin.Events.EventList);
        }
    }
}
