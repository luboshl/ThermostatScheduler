using DotVVM.Diagnostics.StatusPage;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ThermostatScheduler.WebApp
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add(Routes.Default, "", "Pages/Default/Default.dothtml");
            config.RouteTable.Add(Routes.HeatingZones.HeatingZoneCreate, "heating-zones/create", "Pages/HeatingZones/HeatingZoneDetail/HeatingZoneDetail.dothtml");
            config.RouteTable.Add(Routes.HeatingZones.HeatingZoneEdit, "heating-zones/edit/{Id}", "Pages/HeatingZones/HeatingZoneDetail/HeatingZoneDetail.dothtml");
            config.RouteTable.Add(Routes.ScheduledEvents.ScheduledEventCreate, "scheduled-events/create", "Pages/ScheduledEvents/ScheduledEventDetail/ScheduledEventDetail.dothtml");
            config.RouteTable.Add(Routes.ScheduledEvents.ScheduledEventEdit, "scheduled-events/edit/{Id}", "Pages/ScheduledEvents/ScheduledEventDetail/ScheduledEventDetail.dothtml");
            config.RouteTable.AutoDiscoverRoutes(new CustomRouteStrategy(config, "Pages"));
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            //config.Resources.Register("Styles", new StylesheetResource()
            //{
            //    Location = new UrlResourceLocation("~/styles.css")
            //});
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddDefaultTempStorages("temp");
            options.AddStatusPage();
        }
    }
}
