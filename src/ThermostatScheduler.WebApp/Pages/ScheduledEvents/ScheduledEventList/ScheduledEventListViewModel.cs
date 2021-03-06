using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using ThermostatScheduler.WebApp.Models;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.ScheduledEvents.ScheduledEventList
{
    public class ScheduledEventListViewModel : MasterPageViewModel
    {
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        public GridViewDataSet<ScheduledEventListModel> ScheduledEventList { get; set; } = new();
        public ICollection<HeatingZoneListModel> HeatingZones { get; set; } = null!;
        public int? SelectedHeatingZoneId { get; set; }

        public ScheduledEventListViewModel(IDependencies dependencies,
                                           IScheduledEventService scheduledEventService,
                                           IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.scheduledEventService = scheduledEventService;
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (ScheduledEventList.IsRefreshRequired)
            {
                var scheduledEvents = await scheduledEventService.GetAllAsync();

                if (SelectedHeatingZoneId != null)
                {
                    scheduledEvents = scheduledEvents
                        .Where(x => x.HeatingZoneId == SelectedHeatingZoneId)
                        .ToList();
                }

                ScheduledEventList.LoadFromQueryable(
                    scheduledEvents
                        .OrderBy(x => x.HeatingZoneName)
                        .ThenBy(x => x.Time)
                        .AsQueryable());
            }

            HeatingZones = await heatingZoneService.GetAllAsync();
            await base.PreRender();
        }

        public async Task Delete(int id)
        {
            await scheduledEventService.DeleteAsync(id);
            ScheduledEventList.RequestRefresh();
        }

        public async Task Clone(int id)
        {
            var cloneId = await scheduledEventService.CloneAsync(id);
            Context.RedirectToRoute(Routes.ScheduledEvents.ScheduledEventEdit, new { Id = cloneId });
        }

        public void ReloadData()
        {
            ScheduledEventList.RequestRefresh();
        }
    }
}
