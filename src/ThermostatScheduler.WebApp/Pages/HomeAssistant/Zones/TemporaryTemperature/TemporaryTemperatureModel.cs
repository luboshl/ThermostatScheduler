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

        [Required]
        [Range(0, 999)]
        public int Hours { get; set; }

        [Required]
        [Range(0, 999)]
        public int Minutes { get; set; }

        public TemporaryTemperatureModel(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public TemporaryTemperatureModel()
        {
        }
    }
}
