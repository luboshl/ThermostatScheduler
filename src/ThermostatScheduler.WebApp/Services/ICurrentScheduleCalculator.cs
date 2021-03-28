using System.Collections.Generic;
using ThermostatScheduler.Persistence.Model;

namespace ThermostatScheduler.WebApp.Services
{
    public interface ICurrentScheduleCalculator
    {
        ICollection<ScheduledEvent> GetCurrentSchedules(ICollection<ScheduledEvent> scheduledEvents);
    }
}
