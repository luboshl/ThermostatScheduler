namespace ThermostatScheduler.WebApp.Pages.Admin.Zones.ZoneList
{
    public class ZoneListListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public ZoneListListModel(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public ZoneListListModel()
        {
        }
    }
}
