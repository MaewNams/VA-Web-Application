using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;
using VA.Service;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VA.Controllers
{
    public class HomeController : Controller
    {
        private VAContext _db = new VAContext();
        private AdministratorRepository AdminService = new AdministratorRepository();
        private MemberRepository MemberService = new MemberRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private SMSService smsService = new SMSService();
        private PetRepository PetService = new PetRepository();
        private VAProfileRepository VAService = new VAProfileRepository();


        public ActionResult Index( int? day,int? month, int? year)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (day == null)
            { 
                day = 6;
            }
            if (month == null)
            {
                month = 4;
            }
            if (year == null)
            {
                year = 2017;
            }
            ViewData["MonthAppointmentWait"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Waiting");
            ViewData["MonthAppointmentCom"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Complete");
            ViewData["DialyAppointmentWait"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Waiting");
            ViewData["DialyAppointmentCom"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete");
            var dateTime = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.DateTime = dateTime;
            ViewBag.Day = day.Value;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
            ViewData["AllMembers"] = MemberService.GetAll();
            return View();
        }

        public ActionResult Monthly(int? day,int? month, int? year)
        {
            if (day == null)
            {
                day = DateTime.Now.Day;
            }
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            ViewData["MonthAppointmentWait"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Waiting");
            ViewData["MonthAppointmentCom"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Complete");
            ViewData["DialyAppointmentWait"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Waiting");
            ViewData["DialyAppointmentCom"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete");
            var dateTime = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.DateTime = dateTime;
            ViewBag.Day = day.Value;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
            ViewBag.Test = new DateTime(year.Value, month.Value, day.Value);
            ViewData["AllMembers"] = MemberService.GetAll();
            Session["Authen"] = true;
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
                return View();
        }


        public ActionResult Contact(int? day, int? month, int? year)
        {
            if (day == null)
            {
                day = DateTime.Now.Day;
            }
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            ViewData["MonthAppointmentWait"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Waiting");
            ViewData["MonthAppointmentCom"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Complete");
            ViewData["DialyAppointmentWait"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Waiting");
            ViewData["DialyAppointmentCom"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete");
            var dateTime = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.DateTime = dateTime;
            ViewBag.Day = day.Value;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
                return View();
        }

        public ActionResult VAProfile()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Clinic clinic = VAService.Get();
            ViewBag.Message = "Your contact page.";

            return View(clinic);
        }


        public ActionResult Member()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewData["AllMember"] = MemberService.GetAll().OrderBy(m => m.id);
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Appointment(int? day ,int? month, int? year)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (day == null)
            {
                day = DateTime.Now.Day;
            }
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }

            ViewData["MonthAppointmentWait"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Waiting");
            ViewData["MonthAppointmentCom"] = AppointmentService.GetByMonthAndYear(month.Value, year.Value, "Complete");
            ViewData["DialyAppointmentWait"] = AppointmentService.GetByDayAndMonthAndYear(day.Value,month.Value, year.Value, "Waiting");
            ViewData["DialyAppointmentCom"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete");
            var dateTime = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.DateTime = dateTime;
            ViewBag.Day = day.Value;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);

            return View();
        }


        public ActionResult Login()
        {
            ViewBag.Message = "Your Login page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "username,password")] Administrator administrator)
        {

            var Authentication = AdminService.GetByUsernamrAndPassword(administrator.username, administrator.password);


            if (Authentication != null)
            {
                Session["Authen"] = true;
                Session["username"] = Authentication.username.ToString();
                Session["accountid"] = Authentication.id;

             

                /* ViewBag.LoginSuccess = "Login success!";
                return View();*/
                return RedirectToAction("Index");
            }
            else
            {
                /*   Session["username"] = null;
                    Session["accountid"] = null;*/
                ViewBag.LoginError = "Username or password is in correct";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["Authen"] = null;
            Session["Username"] = null;
            Session["AccountID"] = null;

            ViewBag.LogoutSuccess = "Logout success!";
            /*    return View();*/

            return RedirectToAction("Login");

        }
        [Route("Home/SearchMember")]
        public ActionResult SearchMember()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Message = "Your Login page.";

            return View();
        }
        [Route("Home/Member/{condition}")]
        [Route("Home/Member/{condition}/{keyword}")]
        public ActionResult SearchMember(string condition, string keyword)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if ( condition == "idCode" ) {
            /*    ViewData["Members"] = MemberService.GetByCodeID(keyword);*/   
                return View();
            }

            if (condition == "name")
            {
                ViewData["Members"] = MemberService.GetByName(keyword);
                return View();
            }

            if (condition == "address")
            {
                ViewData["Members"] = MemberService.GetByAddress(keyword);
                return View();
            }
            if (condition == "phone") {
                ViewData["Members"] = MemberService.GetByPhoneNumber(keyword);
                return View();
            }

  

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditClinic(string address, string phoneNumber, string open)
        {
            Boolean isNumber = Regex.IsMatch(phoneNumber, @"^\d+$");
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { Result = "Fail, address is required" });
            }
            if (String.IsNullOrEmpty(open))
            {
                return Json(new { Result = "Fail, Open detail is required" });
            }
   
            if (String.IsNullOrEmpty(phoneNumber))
            {
                return Json(new { Result = "Fail, phone number is required" });
            }
            if (isNumber == false)
            {
                return Json(new { Result = "Fail, phone number can only contain 0-9" });
            }
            if (phoneNumber.Length != 10)
            {
                return Json(new { Result = "Fail, phone number have to contain 10 numeric character" });
            }

            Clinic clinic = VAService.Get();
            clinic.address = address;
            clinic.phonenumber = phoneNumber;
            clinic.openingDetail = open;
            VAService.Update(clinic);
            Debug.WriteLine(clinic.address + "first");
            return Json(new { Result = "Success" });
        }


    }
}