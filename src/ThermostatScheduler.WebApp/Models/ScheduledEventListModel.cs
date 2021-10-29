﻿using System;

namespace ThermostatScheduler.WebApp.Models
{
    public class ScheduledEventListModel
    {
        public int Id { get; set; }
        public int HeatingZoneId { get; set; }
        public string HeatingZoneName { get; set; } = null!;
        public DateTime Time { get; set; }
        public string? Temperature { get; set; }
        public bool IsActive { get; set; }

        public ScheduledEventListModel(int id,
                                       int heatingZoneId,
                                       string heatingZoneName,
                                       DateTime time,
                                       double? temperature,
                                       bool isActive)
        {
            Id = id;
            HeatingZoneId = heatingZoneId;
            HeatingZoneName = heatingZoneName;
            Time = time;
            Temperature = temperature != null ? temperature?.ToString("N1") + "°C" : null;
            IsActive = isActive;
        }

        public ScheduledEventListModel()
        {
        }
    }
}
