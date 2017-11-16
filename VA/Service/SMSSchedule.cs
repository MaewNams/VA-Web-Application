using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Twilio;
using Quartz.Impl;
using System.IO;
using System.Web.UI;

namespace VA.Service
{
    public class SMSSchedule
    {

        public static void Start()
        {

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();

            IJobDetail SMSJob = JobBuilder.Create<SMSService>()
                .WithIdentity("SMS")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
          /*     .WithDailyTimeIntervalSchedule
                (s =>
                s.WithIntervalInMinutes(1)
                .OnEveryDay()
            )*/
            .ForJob(SMSJob)
            .WithIdentity("trigger")
            .WithCronSchedule("0 0/1 * * * ?") //This one use for test, fire every 1 minute
           //  .WithCronSchedule("0 30 5,13,17,21 * * ?") //This one fire at 9.30,13.30,17.30, and 21.30
             .StartNow()
            .Build();


            scheduler.ScheduleJob(SMSJob, trigger);
            scheduler.Start();


        }/*

        /*    static void Main(string[] args)
            {
                Test();
            }
/*
            public static void Test()
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                IScheduler scheduler = schedulerFactory.GetScheduler();

                IJobDetail jobDetail = JobBuilder.Create<SMSService>()
                    .WithIdentity("TestJob")
                    .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .ForJob(jobDetail)
                  .WithSimpleSchedule(x => x
        .WithIntervalInMinutes(1)
        .RepeatForever())
                    .WithIdentity("TestTrigger")
                    .StartNow()
                    .Build();
                scheduler.ScheduleJob(jobDetail, trigger);
                scheduler.Start();
            }
        }

     /*    internal class SatellitePaymentGenerationJob : IJob
         {
             public void Execute(IJobExecutionContext context)
             {
            // set our AccountSid and AuthToken
            string AccountSid = "AC25195d70ba44c681b9b24a78ea22a4ff";
            string AuthToken = "9f9059fd2c66671cf5bc6be265946c4e";

           // string message;
            //   string phone = "950828781";

            // instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(AccountSid, AuthToken);

            //test sending message
            string message2 = "Meet again at World Next year!";
            string phone = "950828781";
            string name = "IBoy";
            var result = client.SendMessage("619-473-7049", "+66" + phone, string.Format("Hello {0}, tomorrow you have appointment with us! " + message2, name));

        }
    }*/
    }
}