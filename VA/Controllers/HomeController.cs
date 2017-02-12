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

namespace VA.Controllers
{
    public class HomeController : Controller
    {
        private VAContext _db = new VAContext();
        private AdministratorRepository AdminService = new AdministratorRepository();
        private MemberRepository MemberService = new MemberRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private SMSService smsService = new SMSService();

        public ActionResult Index( int? day,int? month, int? year)
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
            ViewData["AllMembers"] = MemberService.GetAll();
            Session["Authen"] = true;
            if (Session["Authen"] == null) {
                return RedirectToAction("Login", "Home");
            }
            else
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Appointment(int? day ,int? month, int? year)
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
            ViewData["DialyAppointmentWait"] = AppointmentService.GetByDayAndMonthAndYear(day.Value,month.Value, year.Value, "Waiting");
            ViewData["DialyAppointmentCom"] = AppointmentService.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete");
            var dateTime = new DateTime(year.Value, month.Value, 1);
            ViewBag.DateTime = dateTime;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;

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
                ViewBag.LoginError = "Your username or password may be wrong!";
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
            ViewBag.Message = "Your Login page.";

            return View();
        }
        [Route("Home/SearchMember/{condition}")]
        [Route("Home/SearchMember/{condition}/{keyword}")]
        public ActionResult SearchMember(string condition, string keyword)
        {


            if ( condition == "idCode" ) {
                ViewData["Members"] = MemberService.GetByCodeID(keyword);
                return View();
            }

            if (condition == "name")
            {
                ViewData["Members"] = MemberService.QueryByName(keyword);
                return View();
            }

            if (condition == "address")
            {
                ViewData["Members"] = MemberService.QueryByAddress(keyword);
                return View();
            }
            if (condition == "phone") {
                ViewData["Members"] = MemberService.GetByPhoneNumber(keyword);
                return View();
            }

  

            return View();
        }

        }
}