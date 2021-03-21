using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.BusinessPack.Controls;
using Scheduler.App.Models;
using Scheduler.App.Services;

namespace Scheduler.App.Pages.ScheduledEvents.ScheduledEventList
{
    public class ScheduledEventListViewModel : MasterPageViewModel
    {
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        public BusinessPackDataSet<ScheduledEventListModel> ScheduledEventList { get; set; } = new();
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

        public void ReloadData()
        {
            ScheduledEventList.RequestRefresh();
        }
    }
}
