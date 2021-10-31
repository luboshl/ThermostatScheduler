using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ThermostatScheduler.WebApp
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    public static class Routes
    {
        public static string Default = GetRouteName();

        public static class Admin
        {
            public static string GroupName => nameof(Admin);
            public static string GroupPath => $"{GroupName}";

            public static class Zones
            {
                public static string GroupName => nameof(Zones);
                public static string GroupPath => $"{Admin.GroupPath}_{GroupName}";

                public static string ZoneList => GetRouteName(GroupPath);
                public static string ZoneCreate => GetRouteName(GroupPath);
                public static string ZoneEdit => GetRouteName(GroupPath);
            }

            public static class Events
            {
                public static string GroupName => nameof(Events);
                public static string GroupPath => $"{Admin.GroupPath}_{GroupName}";

                public static string EventList => GetRouteName(GroupPath);
                public static string EventCreate => GetRouteName(GroupPath);
                public static string EventEdit => GetRouteName(GroupPath);
            }
        }

        public static class HomeAssistant
        {
            public static string GroupName => nameof(HomeAssistant);
            public static string GroupPath => $"{GroupName}";

            public static class Zones
            {
                public static string GroupName => nameof(Zones);
                public static string GroupPath => $"{HomeAssistant.GroupPath}_{GroupName}";

                public static string TemporaryTemperature => GetRouteName(GroupPath);
            }
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
