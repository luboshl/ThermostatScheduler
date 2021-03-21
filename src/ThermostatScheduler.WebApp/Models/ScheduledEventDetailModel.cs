using System;

namespace Scheduler.App.Models
{
    public class ScheduledEventDetailModel
    {
        public int Id { get; set; }
        public int HeatingZoneId { get; set; }
        public string HeatingZoneName { get; set; } = null!;
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public string Note { get; set; } = null!;

        public ScheduledEventDetailModel(int id, int heatingZoneId, string heatingZoneName, DateTime time, double temperature, string note)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature;
            Note = note;
        }

        public ScheduledEventDetailModel()
        {
        }
    }
}
