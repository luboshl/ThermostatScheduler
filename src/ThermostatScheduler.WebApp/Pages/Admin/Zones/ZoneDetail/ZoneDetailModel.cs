using System.ComponentModel.DataAnnotations;

namespace ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneDetail
{
    public class ZoneDetailModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public ZoneDetailModel(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public ZoneDetailModel()
        {
        }
    }
}
