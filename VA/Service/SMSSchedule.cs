using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.IO;
using System.Web.UI;

namespace VA.Service
{
    public class SMSSchedule
    {
    /*    public static void Start()
        {
            IJobDetail SMSJob = JobBuilder.Create<SMSService>()
                .WithIdentity("SMS")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s =>
                s.WithIntervalInMinutes(1)
                .OnEveryDay()
            )
            .ForJob(SMSJob)
            .WithIdentity("trigger")
            .StartNow()
            .WithCronSchedule("0 0/1 * * * ?")
            .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(SMSJob, trigger);
            sc.Start();
        }*/
    }
}