using DotVVM.Diagnostics.StatusPage;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
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
            
            // Admin
            config.RouteTable.Add(Routes.Admin.Zones.ZoneCreate, "admin/zones/create", "Pages/Admin/Zones/ZoneDetail/ZoneDetail.dothtml");
            config.RouteTable.Add(Routes.Admin.Zones.ZoneEdit, "admin/zones/edit/{Id}", "Pages/Admin/Zones/ZoneDetail/ZoneDetail.dothtml");
            config.RouteTable.Add(Routes.Admin.Events.EventCreate, "admin/events/create", "Pages/Admin/Events/EventDetail/EventDetail.dothtml");
            config.RouteTable.Add(Routes.Admin.Events.EventEdit, "admin/events/edit/{Id}", "Pages/Admin/Events/EventDetail/EventDetail.dothtml");
            
            // HomeAssistant
            config.RouteTable.Add(Routes.HomeAssistant.Zones.TemporaryTemperature, "ha/zones/temporary-temperature/{Code}", "Pages/HomeAssistant/Zones/TemporaryTemperature/TemporaryTemperature.dothtml");

            config.RouteTable.AutoDiscoverRoutes(new CustomRouteStrategy(config, "Pages"));
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register("Styles", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/css/styles.css")
            });
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddDefaultTempStorages("temp");
            options.AddStatusPage();
        }
    }
}
