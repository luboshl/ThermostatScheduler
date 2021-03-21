namespace ThermostatScheduler.Persistence.Model
{
    public class HeatingZone : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public HeatingZone(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
