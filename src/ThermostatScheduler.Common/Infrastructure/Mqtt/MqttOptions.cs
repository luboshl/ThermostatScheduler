using System;

namespace ThermostatScheduler.Common.Infrastructure.Mqtt
{
    public class MqttOptions
    {
        public static string Name = "Mqtt";

        public string ClientId { get; set; } = null!;
        public string ServerAddress { get; set; } = null!;
        public int ServerPort { get; set; }
        public TimeSpan CommunicationTimeout { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
