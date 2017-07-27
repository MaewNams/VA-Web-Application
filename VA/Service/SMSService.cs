using System;
using System.Collections.Generic;
using Twilio;

using Quartz;
using Quartz.Impl;
using System.IO;
using System.Web.UI;
using VA.DAL;
using VA.Repositories;
using System.Linq;

namespace VA.Service

{
    public class SMSService : IJob
    {
        private VAContext _db = new VAContext();
        private TimeBlockRepository TimeBlockService = new TimeBlockRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private VCRepository VCService = new VCRepository();

        public void Execute(IJobExecutionContext context)
        {



            //test sending message
            /*   string message = "Hello word, Aew Aew Aew Aew";
               string phone = "950828781";
               string name = "Faker";
               var result = client.SendMessage("412-413-9257", "+66" + phone, string.Format("Hello {0}, tomorrow you have appointment with us! " + message, name));
             */

            //int day = DateTime.Now.Day + 1;
            //int month = DateTime.Now.Month;
            //int year = DateTime.Now.Year;
              var appointments = AppointmentService.GetByDayAndMonthAndYearAndStatus(8, 7, 2017, "Waiting").ToList();

            // set our AccountSid and AuthToken
            string AccountSid = "AC25195d70ba44c681b9b24a78ea22a4ff";
            string AuthToken = "9f9059fd2c66671cf5bc6be265946c4e";

            string message;
            string phone = "950828781";

            // instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(AccountSid, AuthToken);

            if (appointments != null && appointments.ToList().Count > 0)
            {   
                foreach (var appointment in appointments)
                    {
                        message = "Please bring " + appointment.Pet.name + " to clinic to get " + appointment.detail +
                                        ". Suggestion before go to clinic : " + appointment.suggestion;
                    //  phone = appointment.Member.phonenumber.Substring(1);
                    client.SendMessage("412-413-9257", "+66" + phone, string.Format("Hello " + appointment.Member.name + 
                        ", tomorrow ("+appointment.startTime.Date.ToString("D")+ ") you have appointment with us! Please bring " + appointment.Pet.name + 
                        " to clinic to get" + appointment.VAService.description + 
                        " at " +appointment.startTime + "-" + appointment.endTime)+
                        "\r\n" + "Suggestion before go to the veterinary clinic: " +appointment.suggestion);
                        appointment.status = "Complete";
                        AppointmentService.Update(appointment);

                    }

                }
        }

      /*  public void Send(string phoneNumber, string name, string message)
        {
            // set our AccountSid and AuthToken
            string AccountSid = "AC25195d70ba44c681b9b24a78ea22a4ff";
            string AuthToken = "9f9059fd2c66671cf5bc6be265946c4e";

            // instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(AccountSid, AuthToken);
            // make an associative array of people we know, indexed by phone number
            var result = client.SendMessage("412-413-9257", phoneNumber, string.Format("Hello {0}, tomorrow you have appointment with us! " + message, name));

        }*/
    }
}