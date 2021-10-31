using System.Runtime.CompilerServices;

namespace ThermostatScheduler.WebApp
{
    public static class Routes
    {
        public static string Default = GetRouteName();

        public static class HeatingZones
        {
            public static string GroupName => nameof(HeatingZones);
            public static string GroupPath => $"{GroupName}";

            public static string HeatingZoneList => GetRouteName(GroupPath);
            public static string HeatingZoneCreate=> GetRouteName(GroupPath);
            public static string HeatingZoneEdit => GetRouteName(GroupPath);
        }

        public static class ScheduledEvents
        {
            public static string GroupName => nameof(ScheduledEvents);
            public static string GroupPath => $"{GroupName}";

            public static string ScheduledEventList => GetRouteName(GroupPath);
            public static string ScheduledEventCreate => GetRouteName(GroupPath);
            public static string ScheduledEventEdit => GetRouteName(GroupPath);
        }

        private static string GetRouteName(string groupPrefix = "", [CallerMemberName] string pageName = "")
        {
            if (string.IsNullOrEmpty(groupPrefix))
            {
                return pageName;
            }
            else
            {
                return $"{groupPrefix}_{pageName}";
            }
        }
    }
}
