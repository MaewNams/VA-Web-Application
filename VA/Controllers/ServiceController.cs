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
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private PetRepository PetService = new PetRepository();
        private SpecieRepository SpecieService = new SpecieRepository();
        private TimeSlotRepository TimeSlotService = new TimeSlotRepository();
        private AppTimeRepository AppTimeService = new AppTimeRepository();
        private  VCRepository VCService = new VCRepository();


        // GET: Service

        /*public ActionResult SMSSend()
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
        }*/

        [HttpPost]
        public ActionResult TruncateDB()
        {
            //      _db.ExecuteCommand("TRUNCATE TABLE Entity");

            _db.AppointmentTimeSlot.RemoveRange(_db.AppointmentTimeSlot);
            _db.Appointment.RemoveRange(_db.Appointment);
            _db.Pet.RemoveRange(_db.Pet);
            _db.Specie.RemoveRange(_db.Specie);
            _db.Member.RemoveRange(_db.Member);
            _db.TimeSlot.RemoveRange(_db.TimeSlot);
            _db.SaveChanges();

            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Appointment', RESEED, 0)");
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('AppointmentTimeSlot', RESEED, 0)");
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Pet', RESEED, 0)");
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Specie', RESEED, 0)");
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Member', RESEED, 0)");
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TimeSlot', RESEED, 0)");



            //   DateTime current = DateTime.Now;
            // int day = DateTime.Now.Day;
            //  DateTime year;

            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 00, 0);

            //Start from monday -> sunday

            DateTime today = new DateTime(2017, 11, 15);

            // lastMonday is always the Monday before nextSunday.
            // When date is a Sunday, lastMonday will be tomorrow.     
            int offset = today.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = today.AddDays(-offset);
            DateTime endWeek = lastMonday.AddDays(6);


            //  var monday = today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            //Loop for this week
            while (lastMonday <= endWeek)
            {
                //Start from monday -> sunday

                DateTime dt = new DateTime(lastMonday.Year, lastMonday.Month, lastMonday.Day);
                //Loop for hour in day

                DateTime startTime = new DateTime(dt.Year, dt.Month, dt.Day) + start;
                DateTime endTime = new DateTime(dt.Year, dt.Month, dt.Day) + end;
                var hours = new List<DateTime>();
                hours.Add(startTime);
                var next = new DateTime(startTime.Year, startTime.Month, startTime.Day,
                                        startTime.Hour, startTime.Minute, 0, startTime.Kind);

                while ((next = next.AddHours(0.5)) < endTime)
                {
                    hours.Add(next);
                }
                hours.Add(endTime);

                foreach (var hour in hours)
                {
                    TimeSlot timeblock = new TimeSlot();
                    timeblock.startTime = hour;
                    timeblock.endTime = hour.AddHours(0.5);
                    timeblock.numberofCase = 0;
                    timeblock.status = "Free";
                    TimeSlotService.Add(timeblock);
                    var check = timeblock.id;
                }
                lastMonday = lastMonday.AddDays(1);
            }




            // Add member
            List<Member> members = new List<Member>()
            {
                new Member {email="one@mail.com", password="123456", name ="one", surname ="someone", address ="t1 address", phonenumber="0950828781" },
                 new Member {email="two@mail.com",  password="A12345", name ="two", surname ="tomorrow", address ="t2 address", phonenumber="0222222222" },
                  new Member {email="three@mail.com",  password="654321", name ="three", surname ="teatime", address ="t3 home", phonenumber="0333333333" },

            };
            foreach (Member member in members)
            {
                MemberService.Add(member);
            }


            // Add type
            List<Specie> types = new List<Specie>()
            {
                new Specie {name ="cat"},
                new Specie {name ="dog"},
                new Specie {name ="rabbit"},
                   new Specie {name ="fish"}
            };
            foreach (Specie type in types)
            {
                SpecieService.Add(type);
            }


            /* // Add type
             List<VAService> services = new List<VAService>()
             {
                 new VAService {description ="Vaccination"},
                 new VAService {description ="Health check"},
                 new VAService {description ="Fllow up"},
                 new VAService {description ="Surgery"},
             };
             foreach (VAService service in services)
             {
                 VAService.Add(service);
             }*/

            // Add pet
            List<Pet> pets = new List<Pet>()
            {
                new Pet { memberId = 1,  name ="oneDog", specieId = 2 },
                new Pet { memberId = 1,  name ="oneCat", specieId = 1 },
                new Pet { memberId = 2,  name ="twoRabbit", specieId = 3 },
            };
            foreach (Pet pet in pets)
            {
                PetService.Add(pet);
            }

            //Update clinic
              Clinic clinic = VCService.Get();
              clinic.maximumCase = 2;
              VCService.Update(clinic);
            
            // Add appointment
            //date for 7 aug
            TimeSlot a = TimeSlotService.GetByID(1);
            TimeSlot b = TimeSlotService.GetByID(2);
            TimeSlot c = TimeSlotService.GetByID(8);
            //
            TimeSlot d = TimeSlotService.GetByID(9);
            TimeSlot e = TimeSlotService.GetByID(10);
            //
            TimeSlot f = TimeSlotService.GetByID(11);
            TimeSlot g = TimeSlotService.GetByID(14);
            TimeSlot h = TimeSlotService.GetByID(26);

            List<Appointment> appointments = new List<Appointment>()
                    {
                        new Appointment { memberId = 1, petId =2, serviceId = 1, detail = "Rabies vaccine",
                            suggestion = "",
                            startTime = a.startTime , endTime = a.endTime , status ="Complete" },
                        new Appointment {memberId = 2, petId =3, serviceId = 2, detail = "",
                            suggestion = "",
                            startTime = a.startTime, endTime = b.endTime, status ="Complete" },
                        new Appointment {memberId = 1, petId =1, serviceId = 4, detail = "Sterilization",
                            suggestion = "Stop drinking and eating 4 hour before come to the clinic",
                             startTime =c.startTime , endTime = f.endTime, status ="Waiting" },
                        new Appointment { memberId = 2, petId =3,serviceId = 3, detail = "",
                            suggestion = "",
                            startTime = g.startTime, endTime = g.endTime ,status ="Waiting" },
                        new Appointment { memberId = 2, petId =3,serviceId = 4, detail = "",
                            suggestion = "",
                             startTime =h.startTime , endTime =h.endTime ,status ="Waiting" }
                    };
            foreach (Appointment appointment in appointments)
            {
                AppointmentService.Add(appointment);

            }

            // Update timeslot's number of case and status
            a.numberofCase = 2;
            a.status = "Busy";
            b.numberofCase = 1;
            c.numberofCase = 1;
            c.status = "Full";
            d.numberofCase = 1;
            d.status = "Full";
            e.numberofCase = 1;
            e.status = "Full";
            f.numberofCase = 1;
            f.status = "Full";
            g.numberofCase = 1;
            h.numberofCase = 1;
            h.status = "Full";
            TimeSlotService.Update(a);
            TimeSlotService.Update(b);
            TimeSlotService.Update(c);
            TimeSlotService.Update(d);
            TimeSlotService.Update(e);
            TimeSlotService.Update(f);
            TimeSlotService.Update(g);
            TimeSlotService.Update(h);




            //AppointmentTimeSlot

            // Add pet
            List<AppointmentTimeSlot> apptimes = new List<AppointmentTimeSlot>()
            {

                new AppointmentTimeSlot { timeId = 1,  appointmentID = 1 },
                new AppointmentTimeSlot { timeId = 1,  appointmentID = 2 },
                new AppointmentTimeSlot { timeId = 2,  appointmentID = 2 },
                new AppointmentTimeSlot { timeId = 8,  appointmentID = 3 },
                new AppointmentTimeSlot { timeId = 9,  appointmentID = 3 },
                new AppointmentTimeSlot { timeId = 10,  appointmentID = 3 },
                new AppointmentTimeSlot { timeId = 11,  appointmentID = 3 },
                new AppointmentTimeSlot { timeId = 14,  appointmentID = 4 },
                new AppointmentTimeSlot { timeId = 26,  appointmentID = 5 }
            };
            foreach (AppointmentTimeSlot app in apptimes)
            {
                AppTimeService.Add(app);
            }
            return Json(new { Result = "Success" });
        }
    }
}