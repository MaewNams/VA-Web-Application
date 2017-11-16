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
        private AppointmentRepository AppointmentService = new AppointmentRepository();

//        private readonly IAppointmentRepository _AppRepo;

 
        public void Execute(IJobExecutionContext context)
        {


            // set our AccountSid and AuthToken
            string AccountSid = "AC25195d70ba44c681b9b24a78ea22a4ff";
            string AuthToken = "9f9059fd2c66671cf5bc6be265946c4e";
            string message;

            //   string phone = "950828781";

            // instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(AccountSid, AuthToken);

    /*    //test sending message
            string message2 = "Meet again at World Next year!";
            string phone = "950828781";
            string name = "IBoy";
            var result = client.SendMessage("619-473-7049", "+66" + phone, string.Format("Hello {0}," + message2, name));
*/



                    int day = DateTime.Now.Day+1;
                      int month = DateTime.Now.Month;
                      int year = DateTime.Now.Year;
                      var appointments = AppointmentService.GetByDayAndMonthAndYearAndStatus(day, month, year, "Waiting").ToList();



                      if (appointments != null && appointments.ToList().Count > 0)
                      {   
                          foreach (var appointment in appointments)
                              {
                    if (appointment.suggestion == "" )
                    {
                        appointment.suggestion = "None";
                    }
                                  message = "Please bring " + appointment.Pet.name + " to clinic to get " + appointment.VAService.description +
                                                  ". Suggestion before go to clinic : " + appointment.suggestion;
                              //  phone = appointment.Member.phonenumber.Substring(1);
                              client.SendMessage("619-473-7049", "+66" + appointment.Member.phonenumber, 
                                  string.Format("Hello " + appointment.Member.name +" "+ appointment.Member.surname +
                                  ", tomorrow ("+appointment.startTime.Date.ToString("MMM dd, yyyy") + ") you have appointment with us! Please bring [" + appointment.Pet.Specie.name +"]"+ appointment.Pet.name + 
                                  " to clinic to get " + appointment.VAService.description + " (" + appointment.detail +"). "+
                                  " at " +appointment.startTime.ToShortTimeString() + "-" + appointment.endTime.ToShortTimeString()+
                                   "Suggestion: " + appointment.suggestion));
                                  appointment.status = "Complete";
                    AppointmentService.Update(appointment);

                              }

                          }
        }
  /*      public SMSService(IAppointmentRepository appRepository)
        {
            _AppRepo = appRepository;
        }
        */
    }
}