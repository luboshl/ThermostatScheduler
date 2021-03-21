namespace ThermostatScheduler.WebApp.Models
{
    public class HeatingZoneListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public HeatingZoneListModel(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public HeatingZoneListModel()
        {
        }
    }
}
