namespace ThermostatScheduler.WebApp.Pages.HomeAssistant.Zones.TemporaryTemperature
{
    public class DurationItem
    {
        public int TotalMinutes { get; }
        public string Display { get; }

        public DurationItem(int totalMinutes, string display)
        {
            TotalMinutes = totalMinutes;
            Display = display;
        }
    }
}
