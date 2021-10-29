using System;
using System.ComponentModel.DataAnnotations;

namespace ThermostatScheduler.WebApp.Models
{
    public class ScheduledEventDetailModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage="Zóna musí být zvolena")]
        public int HeatingZoneId { get; set; }
        public string HeatingZoneName { get; set; } = null!;

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public double Temperature { get; set; }

        public ScheduledEventDetailModel(int id, int heatingZoneId, string heatingZoneName, DateTime time, double temperature)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature;
        }

        public ScheduledEventDetailModel()
        {
        }
    }
}
