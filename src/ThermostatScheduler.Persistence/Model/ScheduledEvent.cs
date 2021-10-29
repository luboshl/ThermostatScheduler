using System;

namespace ThermostatScheduler.Persistence.Model
{
    public class ScheduledEvent : Entity
    {
        public int HeatingZoneId { get; set; }
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }

        public ScheduledEvent(int heatingZoneId, TimeSpan time, double temperature)
        {
            HeatingZoneId = heatingZoneId;
            Time = time;
            Temperature = temperature;
        }
    }
}
