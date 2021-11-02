using DotVVM.Framework.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Quartz;
using Serilog;
using ThermostatScheduler.Common.Infrastructure;
using ThermostatScheduler.Common.Infrastructure.Mqtt;
using ThermostatScheduler.Common.Settings;
using ThermostatScheduler.Persistence.Repositories;
using ThermostatScheduler.Processing;
using ThermostatScheduler.WebApp.Pages;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AddSerilog(services);
            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();
            services.AddDotVVM<DotvvmStartup>();
            services.AddQuartz(config =>
            {
                config.UseMicrosoftDependencyInjectionJobFactory();
            });
            services
                .AddScoped<ViewModelBase.IDependencies, ViewModelBase.Dependencies>()
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient<IDateTimeProvider, LocalDateTimeProvider>()
                .AddTransient<IMqttClientFactory, MqttFactory>();
            services
                .AddTransient<IHeatingZoneService, HeatingZoneService>()
                .AddTransient<IScheduledEventService, ScheduledEventService>()
                .AddTransient<IThermostatClient, ThermostatMqttClient>()
                .AddTransient<ICurrentScheduleCalculator, CurrentScheduleCalculator>();
            services
                .AddSingleton<SchedulerManager>()
                .AddSingleton<ISchedulerManager>(sp => sp.GetRequiredService<SchedulerManager>())
                .AddTransient<SetTemperatureJob>();
            services
                .AddSingleton<MqttClientAdapter>()
                .AddSingleton<IMqttClientAdapter>(sp => sp.GetRequiredService<MqttClientAdapter>())
                .AddSingleton<IInitializable>(sp => sp.GetRequiredService<MqttClientAdapter>())
                .AddSingleton<IMqttPublisher, MqttPublisher>()
                .Configure<MqttOptions>(configuration.GetSection(MqttOptions.Name));

            services.AddOptions<PersistenceSettings>()
                .Bind(configuration.GetSection(PersistenceSettings.Name))
                .ValidateDataAnnotations();

            services.AddHostedService<SchedulerManager>(sp => sp.GetRequiredService<SchedulerManager>());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(
                            "http://172.16.1.12",
                            "http://172.16.1.164");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
            dotvvmConfiguration.AssertConfigurationIsValid();

            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath)
            });
        }

        private void AddSerilog(IServiceCollection services)
        {
            var serilogConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            Log.Logger = serilogConfiguration
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
        }
    }
}
