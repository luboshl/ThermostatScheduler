using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotVVM.Framework.ViewModel;

namespace ThermostatScheduler.WebApp.Pages.HomeAssistant.Zones.TemporaryTemperature
{
    public class TemporaryTemperatureModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        [Required]
        [Range(1, 30)]
        public double Temperature { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<DurationItem> Durations { get; set; }

        public int TotalMinutes { get; set; } = 5;

        public TemporaryTemperatureModel(int id, string name, string code, List<DurationItem> durations)
        {
            Id = id;
            Name = name;
            Code = code;
            Durations = durations;
        }

        public TemporaryTemperatureModel()
        {
        }
    }
}
