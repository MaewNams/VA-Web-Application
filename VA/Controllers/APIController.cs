using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;
using VA.ViewModel;
namespace VA.Controllers
{
    public class APIController : Controller
    {
       // private VAContext _db = new VAContext();
        private readonly ITimeSlotRepository _TimeSlotRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly IAppointmentRepository _AppRepo;

        public APIController(IAppointmentRepository appRepository,
  IMemberRepository memberRepository, 
              ITimeSlotRepository timeslotRepository)
        {
            _AppRepo = appRepository;
            _MemberRepo = memberRepository;
            _TimeSlotRepo = timeslotRepository;
        }

        public APIController()
        {

        }

        public ActionResult Login(string email, string password)
        {
          
         var login = _MemberRepo.GetByEmailAndPassword(email,password);
            var a = 0 ;
            if (login != null)
            {
                a = login.id;
            }    
            return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MemberProfile(int? memberId)
        {

            var m = _MemberRepo.GetByID(memberId.Value);
            var member = new Member
            {
                id = m.id,
                password = m.password,
                name = m.name,
                surname = m.surname,
                email = m.email,
                address = m.address,
                phonenumber = m.phonenumber
            };
            return Json(JsonConvert.SerializeObject(member), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Appointment(int? memberId,int? month, int? year)
        {
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }

            var appointmentList = _AppRepo.GetByMemberIdAndMonthAndYearNoStatus(memberId.Value, month.Value, year.Value);

            var a = appointmentList.Select(s => new AppointmentViewModel 
            {
                appointmentId = s.id,
                start = s.startTime,
                end = s.endTime,
                petName = s.Pet.name,
                petSpecie = s.Pet.Specie.name,
                service = s.VAService.description,
                detail = s.detail,
                suggestion = s.suggestion
           
            });
           
               return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }

       /* public ActionResult UpcomingAppointment(int? memberId)
        {
            DateTime today = DateTime.Now;

            var appointmentList = _AppRepo.GetByMemberIdAndDayAndMonthAndYearNoStatus(memberId.Value, today.Year, today.Month, today.Day);

            var a = appointmentList.Select(s => new AppointmentViewModel
            {
                appointmentId = s.id,
                start = s.startTime,
                end = s.endTime,
                petName = s.Pet.name,
                petSpecie = s.Pet.Specie.name,
                service = s.VAService.description,
                detail = s.detail,
                suggestion = s.suggestion

            });

            return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }
        */
        public ActionResult CheckTimetble(int?day, int? month, int? year)
        {
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            if(day == null)
            {
                day = DateTime.Now.Day;
            }
            // Check if the system already have that day time table -- if not, create one//
            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 00, 0);
            DateTime today = new DateTime(year.Value, month.Value, day.Value);

            TimeSlot checkExits = _TimeSlotRepo.GetByDate(day.Value, month.Value, year.Value);

            int offset = today.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = today.AddDays(-offset);
            DateTime endWeek = lastMonday.AddDays(6);
            if (checkExits == null)
            {
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
                        _TimeSlotRepo.Add(timeblock);
                    }
                    lastMonday = lastMonday.AddDays(1);
                }
           
            }

            var timetable = _TimeSlotRepo.GetListByDate(day.Value,month.Value, year.Value);

            var t = timetable.Select(s => new TimeSlot
            {
                id = s.id,
                startTime = s.startTime,
                endTime = s.endTime,
                status = s.status
            });
            return Json(JsonConvert.SerializeObject(t), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChangPassword(int? id, string password, string rePassword)
        {
            if (String.IsNullOrEmpty(password))
            {
                return Json(new { Result = "Fail, password is required" }, JsonRequestBehavior.AllowGet);
            }
            Boolean isValid = Regex.IsMatch(password, @"^[0-9A-Za-z]+$");
            if (isValid == false)
            {
                return Json(new { Result = "Fail, password can only contain a-z, A-Z, and 0-9" }, JsonRequestBehavior.AllowGet);
            }
            if (password.Length < 6)
            {
                return Json(new { Result = "Fail, password have to contain atleast 6 character" }, JsonRequestBehavior.AllowGet);
            }
            if(password != rePassword)
            {
                return Json(new { Result = "Fail, password and re-password is not match" }, JsonRequestBehavior.AllowGet);
            }
            else {
            Member member = _MemberRepo.GetByID(id.Value);
            member.password = password;
            _MemberRepo.Update(member);

            return Json(new { Result = "Edit success" }, JsonRequestBehavior.AllowGet);
        }
        }

        public ActionResult ChangMemberInfo(int? id,string email, string name, string surname, string address, string phonenumber)
        {
            Boolean isNumber = Regex.IsMatch(phonenumber, @"^\d+$");
            Member checkmail = _MemberRepo.GetByExactlyEmail(email);
            Member checkname = _MemberRepo.GetByNameAndSurname(name, surname);
            var foo = new EmailAddressAttribute();
            bool bar = foo.IsValid(email);

            if (String.IsNullOrEmpty(email))
            {
                return Json(new { Result = "Fail, email is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, name is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(surname))
            {
                return Json(new { Result = "Fail, surname is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { Result = "Fail, address is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(phonenumber))
            {
                return Json(new { Result = "Fail, phone number is required" }, JsonRequestBehavior.AllowGet);
            }
            if (isNumber == false)
            {
                return Json(new { Result = "Fail, phone number can only contain 0-9" }, JsonRequestBehavior.AllowGet);
            }
            if (phonenumber.Length != 10)
            {
                return Json(new { Result = "Fail, phone number have to contain 10 numeric character" }, JsonRequestBehavior.AllowGet);
            }
            if (bar == false)
            {
                return Json(new { Result = "Fail, email address is in invalid format, the valid format of email is xxx@xxx.xx" }, JsonRequestBehavior.AllowGet);
            }
            if ((checkmail != null) && (checkmail.id != id.Value))
            {
                return Json(new { Result = "Fail, the email is already exits in the system" }, JsonRequestBehavior.AllowGet);
            }
            if ((checkname!= null)&&(checkname.id != id.Value))
            {
                return Json(new { Result = "Fail, member with name "+ name + " and surname " +surname + "is alredy exits in the system" }, JsonRequestBehavior.AllowGet);
            }
            Member member = _MemberRepo.GetByID(id.Value);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.email = email;
            member.phonenumber = phonenumber;
            _MemberRepo.Update(member);
            return Json(new { Result = "Edit success" }, JsonRequestBehavior.AllowGet);
        }
    }
} 