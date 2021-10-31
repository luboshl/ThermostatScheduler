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
            Context.RedirectToRoute(Routes.ScheduledEvents.ScheduledEventList);
            return Task.CompletedTask;
        }
    }
}
