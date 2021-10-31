using System;
using ThermostatScheduler.Common;

namespace ThermostatScheduler.WebApp.Pages.Admin.Events.EventList
{
    public class EventListModel
    {
        public int Id { get; set; }
        public int HeatingZoneId { get; set; }
        public string HeatingZoneName { get; set; } = null!;
        public DateTime Time { get; set; }
        public ScheduleMode Mode { get; }
        public string? Temperature { get; set; }
        public DateTime? ValidFrom { get; }
        public DateTime? ValidTo { get; }
        public string? Validity => GetValidity();

        public bool IsActive { get; set; }

        public EventListModel(int id,
                                       int heatingZoneId,
                                       string heatingZoneName,
                                       DateTime time,
                                       double? temperature,
                                       ScheduleMode mode,
                                       DateTime? validFrom,
                                       DateTime? validTo,
                                       bool isActive)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature != null ? temperature?.ToString("N1") + "°C" : null;
            Mode = mode;
            ValidFrom = validFrom;
            ValidTo = validTo;
            IsActive = isActive;
        }

        public EventListModel()
        {
        }

        private string? GetValidity()
        {
            if (ValidFrom == null && ValidTo == null)
            {
                return null;
            }

            if (Mode == ScheduleMode.OneTimeOnly)
            {
                return $"{GetDate(ValidFrom)}";
            }

            if (ValidFrom != null && ValidTo != null)
            {
                return $"{GetDate(ValidFrom)}—{GetDate(ValidTo)}";
            }

            if (ValidFrom != null)
            {
                return $"{GetDate(ValidFrom)} ->";
            }

            return $"-> {GetDate(ValidTo)}";
        }

        private string? GetDate(DateTime? dateTime)
        {
            return dateTime?.ToString("dd.MM.yyyy");
        }
    }
}
