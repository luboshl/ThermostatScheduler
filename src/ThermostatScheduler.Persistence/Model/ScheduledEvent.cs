using System;

namespace ThermostatScheduler.Persistence.Model
{
    public class ScheduledEvent : Entity
    {
        public int HeatingZoneId { get; set; }
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }
        public string Note { get; set; }

        public ScheduledEvent(int heatingZoneId, TimeSpan time, double temperature, string note)
        {
            HeatingZoneId = heatingZoneId;
            Time = time;
            Temperature = temperature;
            Note = note;
        }
    }
}
