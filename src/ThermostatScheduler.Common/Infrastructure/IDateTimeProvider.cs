using System;

namespace Scheduler.Common.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}