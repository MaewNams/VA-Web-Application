using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VA.Service;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.Controllers
{
    public class ServiceController : Controller
    {
        private readonly SMSService _smsService = new SMSService();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        // GET: Service
        public ActionResult SMSSend()
        {
            int day = DateTime.Now.Day + 1 ;
           int month = DateTime.Now.Month;
          int  year = DateTime.Now.Year;
            var appointments = AppointmentService.GetByDayAndMonthAndYear(day, month, year, "Waiting").ToList();
            
            string message;
            string phone ="823928427";
            if (appointments != null && appointments.ToList().Count > 0)
            {
                foreach (var appointment in appointments)
                {
                     message = "Please bring " + appointment.Pet.name + " to clinic to get " + appointment.detail +
                                     ". Suggestion before go to clinic : " + appointment.suggestion;
                  //  phone = appointment.Member.phonenumber.Substring(1);
                    _smsService.Send("+66" + phone, appointment.Member.name, message);
                       appointment.status = "Complete";
                      AppointmentService.Update(appointment);

                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}