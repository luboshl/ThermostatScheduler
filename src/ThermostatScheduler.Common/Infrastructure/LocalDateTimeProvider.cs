using System;

namespace ThermostatScheduler.Common.Infrastructure
{
    public class LocalDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}