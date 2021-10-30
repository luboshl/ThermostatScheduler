using System;
using ThermostatScheduler.Common;

namespace ThermostatScheduler.Persistence.Model
{
    public class ScheduledEvent : Entity
    {
        public int HeatingZoneId { get; set; }
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public ScheduleMode Mode { get; set; }

        public ScheduledEvent(int heatingZoneId,
                              TimeSpan time,
                              double temperature,
                              DateTime? validFrom,
                              DateTime? validTo,
                              ScheduleMode mode)
        {
            HeatingZoneId = heatingZoneId;
            Time = time;
            Temperature = temperature;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Mode = mode;
        }

        public ScheduledEvent GetClone()
        {
            return new ScheduledEvent(
                HeatingZoneId,
                Time,
                Temperature,
                ValidFrom,
                ValidTo,
                Mode);
        }
    }
}
