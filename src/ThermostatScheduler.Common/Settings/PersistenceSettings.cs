using Newtonsoft.Json;

namespace ThermostatScheduler.Common.Settings
{
    public class PersistenceSettings
    {
        public static string Name = "Persistence";

        [JsonRequired]
        public string FilePath { get; set; } = null!;
    }
}
