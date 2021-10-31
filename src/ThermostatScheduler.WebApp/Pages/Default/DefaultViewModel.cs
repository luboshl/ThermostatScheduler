using System.Threading.Tasks;

namespace ThermostatScheduler.WebApp.Pages.Default
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public DefaultViewModel(IDependencies dependencies)
            : base(dependencies)
        {
        }

        public override Task PreRender()
        {
            Context.RedirectToRoute(Routes.Admin.ScheduledEvents.ScheduledEventList);
            return Task.CompletedTask;
        }
    }
}
