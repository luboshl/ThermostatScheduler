using System;

namespace Scheduler.Common.Infrastructure
{
    public class LocalDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}