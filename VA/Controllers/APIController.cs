using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private VAContext _db = new VAContext();
        private TimeBlockRepository TimeBlockService = new TimeBlockRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private VCRepository VCService = new VCRepository();
        // GET: API

        public ActionResult Login(string email, string password)
        {
          
         var login = MemberService.GetByEmailAndPassword(email,password);
            var a = 0 ;
            if (login != null)
            {
                a = login.id;
            }    
            return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MemberProfile(int? memberId)
        {

            var m = MemberService.GetByID(memberId.Value);
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

        public ActionResult Index(int? memberId,int? month, int? year)
        {
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }

            var appointmentList = AppointmentService.GetByMemberId(memberId.Value, month.Value, year.Value);

            var a = appointmentList.Select(s => new AppointmentViewModel
            {
                appointmentId = s.id,
                date = s.date,
                start = s.startTime,
                end = s.endTime,
                petName = s.Pet.name,
                service = s.VAService.description,
                suggestion = s.suggestion
           
            });
           
               return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTimetble(int? memberId, int?day, int? month, int? year)
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
            TimeBlock checkExits = TimeBlockService.GetByDate(day.Value, month.Value, year.Value);
            if (checkExits == null)
            {
                TimeSpan start = new TimeSpan(09, 30, 0);
                TimeSpan end = new TimeSpan(21, 30, 0);
                DateTime startTime = new DateTime(year.Value, month.Value, day.Value) + start;
                DateTime endTime = new DateTime(year.Value, month.Value, day.Value) + end;
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
                List<TimeBlock> CheckTime = TimeBlockService.GetAll().ToList();
                if (CheckTime.Count() > 0)
                {
                    lastTimeID = CheckTime.LastOrDefault().id;
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

            var timetable = TimeBlockService.GetListByDate(day.Value,month.Value, year.Value);
            return Json(JsonConvert.SerializeObject(timetable), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChangPassword(int id, string password, string rePassword)
        {
            if (String.IsNullOrEmpty(password))
            {
                return Json(new { message = "Fail, password is required" }, JsonRequestBehavior.AllowGet);
            }
            Boolean isValid = Regex.IsMatch(password, @"^[0-9A-Za-z]+$");
            if (isValid == false)
            {
                return Json(new { message = "Fail, password can only contain a-z, A-Z, and 0-9" }, JsonRequestBehavior.AllowGet);
            }
            if (password.Length < 6)
            {
                return Json(new { message = "Fail, password have to contain atleast 6 character" }, JsonRequestBehavior.AllowGet);
            }
            if(password != rePassword)
            {
                return Json(new { message = "Fail, password and re-password is not match" }, JsonRequestBehavior.AllowGet);
            }
            else {
            Member member = MemberService.GetByID(id);
            member.password = password;
            MemberService.Update(member);

            return Json(new { message = "Edit success" }, JsonRequestBehavior.AllowGet);
        }
        }

        public ActionResult ChangMemberInfo(int id,string email, string name, string surname, string address, string phonenumber)
        {
            Boolean isNumber = Regex.IsMatch(phonenumber, @"^\d+$");
            Member checkmail = MemberService.GetByIDAndEmail(id, email);
            Member checkname = MemberService.GetByNameAndSurname(name, surname);
         
            if (String.IsNullOrEmpty(email))
            {
                return Json(new { message = "Fail, email is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { message = "Fail, name is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(surname))
            {
                return Json(new { message = "Fail, surname is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { message = "Fail, address is required" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(phonenumber))
            {
                return Json(new { message = "Fail, phone number is required" }, JsonRequestBehavior.AllowGet);
            }
            if (isNumber == false)
            {
                return Json(new { message = "Fail, phone number can only contain 0-9" }, JsonRequestBehavior.AllowGet);
            }
            if (phonenumber.Length != 10)
            {
                return Json(new { message = "Fail, phone number have to contain 10 numeric character" }, JsonRequestBehavior.AllowGet);
            }
            if (checkmail.id != id)
            {
                return Json(new { message = "Fail, the email is already exits in the system" }, JsonRequestBehavior.AllowGet);
            }
            if (checkname.id != id)
            {
                return Json(new { message = "Fail, member with name"+ name + " and surname " +surname + "is alredy exits in the system" }, JsonRequestBehavior.AllowGet);
            }
            Member member = MemberService.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.email = email;
            member.phonenumber = phonenumber;
            MemberService.Update(member);
            return Json(new { message = "Edit success" }, JsonRequestBehavior.AllowGet);
        }
    }
} 