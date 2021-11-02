using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using ThermostatScheduler.WebApp.Services;

namespace ThermostatScheduler.WebApp.Pages.HomeAssistant.Zones.TemporaryTemperature
{
    public class TemporaryTemperatureViewModel : HomeAssistantMasterPageViewModel
    {
        private readonly IHeatingZoneService heatingZoneService;

        [FromRoute("Code")]
        public string Code { get; set; } = null!;

        public bool ShowSuccessAlert { get; set; }

        public TemporaryTemperatureModel Model { get; set; } = null!;

        public TemporaryTemperatureViewModel(IDependencies dependencies, IHeatingZoneService heatingZoneService)
            : base(dependencies)
        {
            this.heatingZoneService = heatingZoneService;
        }

        public override async Task PreRender()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                throw new InvalidOperationException($"{Code} cannot be empty.");
            }

            var zone = await heatingZoneService.GetNameByCodeAsync(Code);
            var durations = GetDurations();
            Model = new TemporaryTemperatureModel(zone.Id, zone.Name, zone.Code, durations);

            await base.PreRender();
        }

        public async Task Save()
        {
            await heatingZoneService.UpdateAsync(Model.Id, Model.Name, Model.Code);
            ShowSuccessAlert = true;
        }

        [AllowStaticCommand]
        public static int IncreaseDuration(int currentTotalMinutes)
        {
            var durations = GetDurations();
            return durations.FirstOrDefault(x => x.TotalMinutes > currentTotalMinutes)?.TotalMinutes ?? currentTotalMinutes;
        }

        [AllowStaticCommand]
        public static int DecreaseDuration(int currentTotalMinutes)
        {
            var durations = GetDurations();
            return durations.LastOrDefault(x => x.TotalMinutes < currentTotalMinutes)?.TotalMinutes ?? currentTotalMinutes;
        }

        private static List<DurationItem> GetDurations()
        {
            return new List<DurationItem>
            {
                new(1, "1 m"),
                new(5, "5 m"),
                new(10, "10 m"),
                new(15, "15 m"),
                new(30, "30 m"),
                new(45, "45 m"),
                new(60, "1 h"),
                new(90, "1 h 30 m"),
                new(120, "2 h"),
                new(180, "3 h"),
                new(300, "5 h"),
                new(480, "8 h"),
                new(720, "12 h"),
                new(960, "16 h")
            };
        }
    }
}
