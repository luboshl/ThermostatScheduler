using System;

namespace Scheduler.App.Models
{
    public class ScheduledEventListModel
    {
        public int Id { get; set; }
        public int HeatingZoneId { get; set; }
        public string HeatingZoneName { get; set; } = null!;
        public DateTime Time { get; set; }
        public double? Temperature { get; set; }
        public string? Description { get; set; }

        public ScheduledEventListModel(int id, int heatingZoneId, string heatingZoneName, DateTime time, double? temperature, string? description)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature;
            Description = description;
        }

        public ScheduledEventListModel()
        {
        }
    }
}
