﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.Common;

namespace ThermostatScheduler.WebApp.Models
{
    public class ScheduledEventDetailModel : IValidatableObject
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Zóna musí být zvolena")]
        public int HeatingZoneId { get; set; }

        [Bind(Direction.ServerToClient)]
        public string HeatingZoneName { get; set; } = null!;

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [Range(1, 30)]
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
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedScheduleMode == ScheduleMode.OneTimeOnly
                && ValidFrom == null)
            {
                yield return new ValidationResult(
                    "Datum musí být nastaveno.",
                    new[] { nameof(ValidFrom) }
                );
            }
        }
    }
}
