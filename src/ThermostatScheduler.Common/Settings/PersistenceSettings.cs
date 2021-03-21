using Newtonsoft.Json;

namespace Scheduler.Common.Settings
{
    public class PersistenceSettings
    {
        public static string Name = "Persistence";

        [JsonRequired]
        public string FilePath { get; set; } = null!;
    }
}
