using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneList;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.Admin.Events.EventList
{
    public class EventListViewModel : AdminMasterPageViewModel
    {
        private const int AllZonesId = 0;
        private readonly IScheduledEventService scheduledEventService;
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("ZoneId")]
        public int? ZoneId { get; set; }

        public GridViewDataSet<EventListModel> EventList { get; set; } = new();

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<ZoneListListModel> Zones { get; set; } = null!;

        public int SelectedZoneId { get; set; }

        public EventListViewModel(IDependencies dependencies,
                                  IScheduledEventService scheduledEventService,
                                  IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.scheduledEventService = scheduledEventService;
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                SelectedZoneId = ZoneId ?? AllZonesId;
            }

            if (EventList.IsRefreshRequired)
            {
                var scheduledEvents = await scheduledEventService.GetAllAsync();

                if (SelectedZoneId != AllZonesId)
                {
                    scheduledEvents = scheduledEvents
                        .Where(x => x.HeatingZoneId == SelectedZoneId)
                        .ToList();
                }

                EventList.LoadFromQueryable(
                    scheduledEvents
                        .OrderBy(x => x.HeatingZoneName)
                        .ThenBy(x => x.Time)
                        .AsQueryable());
            }

            if (!Context.IsPostBack)
            {
                Zones = (await heatingZoneService.GetAllAsync()).ToList();
                Zones.Insert(0, new ZoneListListModel(AllZonesId, "-- Všechny zóny --", ""));
            }

            await base.PreRender();
        }

        public async Task Delete(int id)
        {
            await scheduledEventService.DeleteAsync(id);
            EventList.RequestRefresh();
        }

        public async Task Clone(int id)
        {
            var cloneId = await scheduledEventService.CloneAsync(id);
            Context.RedirectToRoute(Routes.Admin.Events.EventEdit, new { Id = cloneId });
        }

        public void ReloadData()
        {
            EventList.RequestRefresh();
        }

        public void ChangeFilter(int selectedZoneId)
        {
            EventList.RequestRefresh();
        }
    }
}
