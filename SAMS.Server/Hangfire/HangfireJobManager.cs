using System;
using System.Linq;
using Hangfire;
using Hangfire.Storage;
using SAMS.Common.Helpers;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Hangfire
{
    public static class HangfireJobManager
    {
        public static void RegisterJobs()
        {
            if (!AppSettingsHelper.Current.IsLocal)
            {
                RemoveAllHangfireJobs();
                if (AppSettingsHelper.Current.JobsActivityStatus)
                {
                    //Yeni bir job eklenecekse aşağıya eklenmeli
                    //RecurringJob.AddOrUpdate<IxxxService>("JobId2", xxxService => xxxService.Method2(null), "*/5 * * * *");
                    //RecurringJob.AddOrUpdate<IxxxService>("JobId3", xxxService => xxxService.Method3(), Cron.Never);
                }
            }
        }
        private static void RemoveAllHangfireJobs()
        {
            var hangfireMonitor = JobStorage.Current.GetMonitoringApi();

            //RecurringJobs
            foreach (var recurringJob in JobStorage.Current.GetConnection().GetRecurringJobs())
            {
                RecurringJob.RemoveIfExists(recurringJob.Id);
            }

            //ProcessingJobs
            hangfireMonitor.ProcessingJobs(0, int.MaxValue).ForEach(xx => BackgroundJob.Delete(xx.Key));

            //ScheduledJobs
            hangfireMonitor.ScheduledJobs(0, int.MaxValue).ForEach(xx => BackgroundJob.Delete(xx.Key));

            //EnqueuedJobs
            hangfireMonitor.Queues().ToList().ForEach(xx => hangfireMonitor.EnqueuedJobs(xx.Name, 0, int.MaxValue).ForEach(x => BackgroundJob.Delete(x.Key)));
        }
    }
    public class HangfireJobActivator : JobActivator
    {
        private IServiceProvider _serviceProvider;

        public HangfireJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
