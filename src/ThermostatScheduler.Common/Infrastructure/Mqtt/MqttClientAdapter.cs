using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace ThermostatScheduler.Common.Infrastructure.Mqtt
{
    public class MqttClientAdapter : IInitializable, IMqttClientAdapter
    {
        private int connectionRetryCounter;

        private readonly ILogger<MqttClientAdapter> logger;
        private readonly MqttOptions options;

        public IMqttClient Client { get; }

        public event Func<Task>? Connected;
        public event Func<Task>? Disconnected;

        public MqttClientAdapter(ILogger<MqttClientAdapter> logger,
                                 IOptions<MqttOptions> options,
                                 IMqttClientFactory mqttClientFactory)
        {
            this.logger = logger;
            this.options = options.Value;
            Client = mqttClientFactory.CreateMqttClient();
        }

        public Task InitializeAsync(CancellationToken ct)
        {
            Client.UseConnectedHandler(OnConnected);
            Client.UseDisconnectedHandler(OnDisconnectedAsync);
            return ConnectMqttClientSafeAsync(ct);
        }

        private async Task ConnectMqttClientSafeAsync(CancellationToken ct)
        {
            try
            {
                var mqttClientOptions = GetMqttClientOptions();
                await Client.ConnectAsync(mqttClientOptions, ct);
            }
            catch (Exception ex)
            {
                logger.LogError("MQTT client {clientId} failed to connect to {address}:{port}. {error}", options.ClientId, options.ServerAddress, options.ServerPort, ex.Message);
            }
        }

        private void OnConnected(MqttClientConnectedEventArgs args)
        {
            logger.LogDebug("MQTT client {clientId} connected to {address}:{port}.", options.ClientId, options.ServerAddress, options.ServerPort);
            connectionRetryCounter = 0;
            Connected?.Invoke();
        }

        private async Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs args)
        {
            var delay = GetDelay();

            if (args.Exception != null)
            {
                logger.LogWarning("MQTT client {clientId} disconnected from {address}:{port}, retry in {delay}. {error}", options.ClientId, options.ServerAddress, options.ServerPort, delay, args.Exception.Message);
            }
            else
            {
                logger.LogWarning("MQTT client {clientId} disconnected from {address}:{port}, retry in {delay}.", options.ClientId, options.ServerAddress, options.ServerPort, delay);
            }

            Disconnected?.Invoke();
            await Task.Delay(delay);
            await ConnectMqttClientSafeAsync(CancellationToken.None);
        }

        private TimeSpan GetDelay()
        {
            connectionRetryCounter++;

            return connectionRetryCounter switch
            {
                var value when value < 24 => TimeSpan.FromSeconds(5),
                var value when value < 24 + 10 => TimeSpan.FromSeconds(30),
                _ => TimeSpan.FromMinutes(1)
            };
        }

        private IMqttClientOptions GetMqttClientOptions()
        {
            return new MqttClientOptionsBuilder()
                .WithClientId(this.options.ClientId)
                .WithCredentials(this.options.Username, this.options.Password)
                .WithCommunicationTimeout(this.options.CommunicationTimeout)
                .WithTcpServer(this.options.ServerAddress, this.options.ServerPort)
                //.WithTls()
                .WithCleanSession()
                .Build();
        }
    }
}
