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
        private PetSpecieRepository PetTypeService = new PetSpecieRepository();
        private VAServiceRepository VAService = new VAServiceRepository();
        private VCRepository VCService = new VCRepository();
        private TimeBlockRepository TimeBlockService = new TimeBlockRepository();
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
            _db.Appointment.RemoveRange(_db.Appointment);
            _db.AppointmentTimeBlock.RemoveRange(_db.AppointmentTimeBlock);
            _db.Pet.RemoveRange(_db.Pet);
            _db.Clinic.RemoveRange(_db.Clinic);
            _db.PetType.RemoveRange(_db.PetType);
            _db.Service.RemoveRange(_db.Service);
            _db.Member.RemoveRange(_db.Member);
            _db.TimeBlock.RemoveRange(_db.TimeBlock);
            _db.SaveChanges();


            DateTime current = DateTime.Now;
            int day = DateTime.Now.Day;
            //  DateTime year;

            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 30, 0);


            //Check if the system already have today time table -- if not, create this week time table 
            TimeBlock checkTodayExits = TimeBlockService.GetByDate(current.Day, current.Month, current.Year);
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

            //To day is not exits --> Create for this week 
            if (checkTodayExits == null)
            {
                int lastDay = monday.Day + 6;
                //Loop for this week
                for (int i = monday.Day; i <= lastDay; i++)
                {
                    //Start from monday -> sunday
                    DateTime dt = new DateTime(monday.Year, monday.Month, i);
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
                        int lastTimeID;
                        TimeBlock checkLast = TimeBlockService.GetLast();
                        if (checkLast != null)
                        {
                            lastTimeID = checkLast.id;
                        }
                        else
                        {
                            lastTimeID = 0;
                        }
                        foreach (var hour in hours)
                        {
                            TimeBlock timeblock = new TimeBlock();
                            timeblock.id = lastTimeID + 1;
                            timeblock.startTime = hour;
                            timeblock.endTime = hour.AddHours(0.5);
                            timeblock.numberofCase = 0;
                            timeblock.status = "Free";
                            TimeBlockService.Add(timeblock);
                        }
                }
            }

            /*
            // Add member
            List<Member> members = new List<Member>()
            {
                new Member {id=1, email="email1@mail", password="A0111111111", name ="Alice", surname ="White", address ="Wonderland 11/2", phonenumber="0111111111" },
                 new Member {id=2,email="email2@mail",  password="R0211111113", name ="Redhood", surname ="Red", address ="Forest 123", phonenumber="0211111113" },
                  new Member {id=3,email="email3@mail",  password="M0300000000", name ="Mad", surname ="Hatter", address ="Wonderland 3326", phonenumber="0300000000" },
                   new Member {id=4,email="email4@mail",  password="W0888888888", name ="Wolffy", surname ="Gray", address ="Deep Deep Forest 26", phonenumber="0888888888" }
            };
            foreach (Member member in members)
            {
                MemberService.Add(member);
            }


            // Add type
            List<PetType> types = new List<PetType>()
            {
                new PetType {id=1, name ="cat"},
                new PetType {id=2, name ="dog"},
                new PetType {id=3, name ="rabbit"},
            };
            foreach (PetType type in types)
            {
                PetTypeService.Add(type);
            }


            // Add type
            List<VAService> services = new List<VAService>()
            {
                new VAService {id=1, description ="Vaccination"},
                new VAService {id=2, description ="Fllow up"},
                new VAService {id=3, description ="Surgery"},
            };
            foreach (VAService service in services)
            {
                VAService.Add(service);
            }

            // Add pet
            List<Pet> pets = new List<Pet>()
            {
                new Pet {id=1, memberId = 2,  name ="Little Cat", typeId = 1 },
                new Pet {id=2, memberId = 2,  name ="Big Wolf", typeId = 2 },
                new Pet {id=3, memberId = 1,  name ="White rabbit", typeId = 3 },
                new Pet {id=4, memberId = 3, name ="Black cat", typeId = 1},
                new Pet {id=5, memberId = 1,  name ="Black rabbit", typeId = 3 }
            };
            foreach (Pet pet in pets)
            {
                PetService.Add(pet);
            }

            //Add clinic
            Clinic clinic = new Clinic();
            clinic.maximumCase = 0;
            VCService.Add(clinic);
*/
            // Add appointment
            /*    List<Appointment> appointments = new List<Appointment>()
                    {
                        new Appointment {id=1, date = new DateTime(2017, 4, 6), memberId = 2, petId =1, serviceId = 1, suggestion = "Do not drink and eat 4 hour before come to clinic", status ="Complete" },
                        new Appointment {id =2, date = new DateTime(2017, 4, 6), memberId = 3, petId =4, serviceId = 2, suggestion = "None", status ="Complete" },
                        new Appointment {id =3, date = new DateTime(2017, 4, 10), memberId = 2, petId =2, serviceId = 3, suggestion = "None", status ="Waiting" },
                        new Appointment {id =4, date = new DateTime(2017, 5, 10), memberId = 2, petId =1,serviceId = 1, suggestion = "None", status ="Waiting" }
                    };
                    foreach (Appointment appointment in appointments)
                    {
                      AppointmentService.Add(appointment);

                    }*/

            return Json(new { Result = "Success" });
        }
    }
}