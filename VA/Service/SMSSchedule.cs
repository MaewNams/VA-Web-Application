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
   /*     private VAContext _db = new VAContext();
        private readonly SMSService _smsService = new SMSService();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private PetRepository PetService = new PetRepository();
        private PetSpecieRepository PetTypeService = new PetSpecieRepository();
        private VAServiceRepository VAService = new VAServiceRepository();*/

        public static void Start()
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
            .WithCronSchedule("0 0/1 * * * ?") //This one use for test, fire every 1 minute
            //   .WithCronSchedule("0 30 9,13,17,21 * * ?") //This one fire at 9.30,13.30,17.30, and 21.30
            .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(SMSJob, trigger);
            sc.Start();
        }
    }
}