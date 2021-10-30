using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.Common;

namespace ThermostatScheduler.WebApp.Models
{
    public class ScheduledEventDetailModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Zóna musí být zvolena")]
        public int HeatingZoneId { get; set; }

        public string HeatingZoneName { get; set; } = null!;

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public double Temperature { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public ScheduleMode SelectedScheduleMode { get; set; }

        [Bind(Direction.ServerToClient)]
        public List<ScheduleMode> ScheduleModes => Enum.GetValues<ScheduleMode>().ToList();

        public bool IsValiditySet => ValidFrom != null || ValidTo != null;

        public ScheduledEventDetailModel(int id,
                                         int heatingZoneId,
                                         string heatingZoneName,
                                         DateTime time,
                                         double temperature,
                                         DateTime? validFrom,
                                         DateTime? validTo,
                                         ScheduleMode mode)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature;
            ValidFrom = validFrom;
            ValidTo = validTo;
            SelectedScheduleMode = mode;
        }

        public ScheduledEventDetailModel()
        {
        }
    }
}
