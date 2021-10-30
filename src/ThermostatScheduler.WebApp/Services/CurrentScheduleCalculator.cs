using System;
using System.Collections.Generic;
using System.Linq;
using ThermostatScheduler.Common;
using ThermostatScheduler.Common.Infrastructure;
using ThermostatScheduler.Persistence.Model;

namespace ThermostatScheduler.WebApp.Services
{
    public class CurrentScheduleCalculator : ICurrentScheduleCalculator
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public CurrentScheduleCalculator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public ICollection<ScheduledEvent> GetCurrentSchedules(ICollection<ScheduledEvent> scheduledEvents)
        {
            var eventsByZone = scheduledEvents.GroupBy(x => x.HeatingZoneId);
            var result = new List<ScheduledEvent>();

            foreach (var eventsOfZOne in eventsByZone)
            {
                var latestPreviousEvent = GetLatestPreviousEvent(eventsOfZOne);
                if (latestPreviousEvent != null)
                {
                    result.Add(latestPreviousEvent);
                }
            }

            return result;
        }

        private ScheduledEvent? GetLatestPreviousEvent(IEnumerable<ScheduledEvent> eventsOfZone)
        {
            return eventsOfZone
                .Where(x => x.ValidFrom == null || x.ValidFrom.Value < dateTimeProvider.Now)
                .Where(x => x.ValidTo == null || dateTimeProvider.Now < x.ValidTo.Value)
                .OrderBy(x => CalculatePastDateTime(x.Time))
                .LastOrDefault();
        }

        private DateTime CalculatePastDateTime(TimeSpan time)
        {
            var currentTime = dateTimeProvider.Now.TimeOfDay;

            var day = time < currentTime
                ? DateTime.Today
                : DateTime.Today.AddDays(-1);

            return day.Add(time);
        }
    }
}
