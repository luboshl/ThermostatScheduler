using System;

namespace ThermostatScheduler.Common.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}