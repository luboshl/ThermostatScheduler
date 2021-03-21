using System.ComponentModel.DataAnnotations;

namespace Scheduler.App.Models
{
    public class HeatingZoneDetailModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public HeatingZoneDetailModel(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public HeatingZoneDetailModel()
        {
        }
    }
}
