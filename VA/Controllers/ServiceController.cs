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
        private VAContext _db = new VAContext();
        private readonly SMSService _smsService = new SMSService();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private PetRepository PetService = new PetRepository();
        // GET: Service
        public ActionResult SMSSend()
        {
            int day = DateTime.Now.Day + 1;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var appointments = AppointmentService.GetByDayAndMonthAndYear(day, month, year, "Waiting").ToList();

            string message;
            string phone = "823928427";
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

        [HttpPost]
        public ActionResult TruncateDB()
        {
            _db.Appointment.RemoveRange(_db.Appointment);
            _db.Pet.RemoveRange(_db.Pet);
            _db.Member.RemoveRange(_db.Member);
            _db.SaveChanges();

            // Add member
            List<Member> members = new List<Member>()
            {
                new Member {id=1, codeId = "A1B1C1", password="123456", name ="Alice", surname ="White", address ="Wonderland 11/2", phonenumber="0111111111" },
                 new Member {id=2, codeId = "B2C2D2", password="AA22AA", name ="Redhood", surname ="Red", address ="Forest 123", phonenumber="0211111113" },
                  new Member {id=3, codeId = "A2B3C1", password="654321", name ="Mad", surname ="Hatter", address ="Wonderland 3326", phonenumber="0300000000" },
                   new Member {id=4, codeId = "B2C1D2", password="123ABC", name ="Wolffy", surname ="Gray", address ="Deep Deep Forest 26", phonenumber="0888888888" }
            };
            foreach (Member member in members)
            {
                MemberService.Add(member);
            }


            // Add pet
            List<Pet> pets = new List<Pet>()
            {
                new Pet {id=1, memberId = 2,  name ="Little Cat", typeId = 1 },
                 new Pet {id=2, memberId = 2,  name ="Big Wolf", typeId = 2 },
                  new Pet {id=3, memberId = 1,  name ="White rabbit", typeId = 3 },
                   new Pet {id=4, memberId = 3, name ="Black cat", typeId = 1}
            };
            foreach (Pet pet in pets)
            {
                PetService.Add(pet);
            }



            // Add appointment
            List<Appointment> appointments = new List<Appointment>()
            {
                new Appointment {id=1, date = new DateTime(2017, 4, 6), memberId = 2, petId =1, detail ="Get heart worm vaccine", suggestion = "Do not drink and eat 4 hour before come to clinic", status ="Complete" },
                new Appointment {id =2, date = new DateTime(2017, 4, 6), memberId = 3, petId =4, detail ="Get hrabi vaccine", suggestion = "None", status ="Complete" },
                new Appointment {id =3, date = new DateTime(2017, 4, 10), memberId = 2, petId =2, detail ="Get HIV vaccine", suggestion = "None", status ="Waiting" },
                new Appointment {id =4, date = new DateTime(2017, 5, 10), memberId = 2, petId =1, detail ="Check health", suggestion = "None", status ="Waiting" }
            };
            foreach (Appointment appointment in appointments)
            {
              AppointmentService.Add(appointment);

            }

            return Json(new { Result = "Success" });
        }
    }
}