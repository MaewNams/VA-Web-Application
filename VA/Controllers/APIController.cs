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
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private VAProfileRepository VAProfileService = new VAProfileRepository();
        // GET: API

        public ActionResult Login(string memberIdCode, string password)
        {
          
            var login = MemberService.GetByCodeIDAndPassword(memberIdCode,password);
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
                codeId = m.codeId,
                password = m.password,
                name = m.name,
                surname = m.surname,
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
                petName = s.Pet.name,
                detail = s.detail,
                suggestion = s.suggestion
           
            });
               return Json(JsonConvert.SerializeObject(a), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClinicProfile()
        {
            var c = VAProfileService.Get();

            var clinic = new Clinic
            {
                address = c.address,
                phonenumber = c.phonenumber,
                openingDetail = c.openingDetail
            };
            return Json(JsonConvert.SerializeObject(clinic), JsonRequestBehavior.AllowGet);
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

        public ActionResult ChangMemberInfo(int id, string name, string surname, string address, string phonenumber)
        {
            Boolean isNumber = Regex.IsMatch(phonenumber, @"^\d+$");
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

            Member member = MemberService.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phonenumber;
            MemberService.Update(member);
            return Json(new { message = "Edit success" }, JsonRequestBehavior.AllowGet);
        }
    }
} 