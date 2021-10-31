﻿using System.Diagnostics.CodeAnalysis;
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

            public static class HeatingZones
            {
                public static string GroupName => nameof(HeatingZones);
                public static string GroupPath => $"{Admin.GroupPath}_{GroupName}";

                public static string HeatingZoneList => GetRouteName(GroupPath);
                public static string HeatingZoneCreate => GetRouteName(GroupPath);
                public static string HeatingZoneEdit => GetRouteName(GroupPath);
            }

            public static class ScheduledEvents
            {
                public static string GroupName => nameof(ScheduledEvents);
                public static string GroupPath => $"{Admin.GroupPath}_{GroupName}";

                public static string ScheduledEventList => GetRouteName(GroupPath);
                public static string ScheduledEventCreate => GetRouteName(GroupPath);
                public static string ScheduledEventEdit => GetRouteName(GroupPath);
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
