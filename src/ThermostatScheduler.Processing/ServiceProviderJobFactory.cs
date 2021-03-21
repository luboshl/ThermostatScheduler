using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace ThermostatScheduler.Processing
{
    public class ServiceProviderJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;
            return (IJob)serviceProvider.GetRequiredService(jobType);
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
