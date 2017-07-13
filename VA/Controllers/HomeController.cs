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
        private PetSpecieRepository SpecieService = new PetSpecieRepository();
        private VCRepository VCService = new VCRepository();


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

        public ActionResult VASetting()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Clinic clinic = VCService.Get();
            ViewBag.Message = "Your contact page.";

            return View(clinic);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VASetting(string caseNumber)
        {
            Boolean isNumber = Regex.IsMatch(caseNumber, @"^\d+$");
            if (isNumber == false)
            {
                return Json(new { Result = "Fail, maximum case can only numeric character" });
            }
            Clinic clinic = VCService.Get();
            clinic.maximumCase = Int32.Parse(caseNumber.ToString());
            VCService.Update(clinic);
            return Json(new { Result = "Success" });
        }

        public ActionResult PetSpecie()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewData["PetSpecies"] = SpecieService.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddSpecie(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, specie name is required" });
            }
            PetType type = SpecieService.GetByName(name);
            if(type != null)
            {
                return Json(new { Result = "Fail, this specie is already exits in the system" });
            }

            List<PetType> CheckType = SpecieService.GetAll().ToList();
            int lastID = CheckType.LastOrDefault().id;

            PetType newtype = new PetType();
            newtype.id = lastID + 1;
            newtype.name = name;
            SpecieService.Add(newtype);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSpecie(string typeID, string name)
        {   int id = Int32.Parse(typeID.ToString());

            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, specie name is required" });
            }
            PetType type = SpecieService.GetByName(name);
            if (type != null && type.id != id)
            {
                return Json(new { Result = "Fail, this specie is already exits in the system" });
            }
            PetType editType = SpecieService.GetById(id);
            editType.name = name;
            SpecieService.Update(editType);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteSpecie(string typeID)
        {
            int id = Int32.Parse(typeID.ToString());
            Pet deletePetType = PetService.GetByType(id);


            if (deletePetType != null)
            {
                return Json(new { Result = "Fail, cannot delete the pet specie. Please delete all pet which belong to the specie '"+deletePetType.PetType.name+"' before try again" });
            }
            /////////
            PetType deleteType = SpecieService.GetById(id);
            SpecieService.Delete(deleteType);
            return Json(new { Result = "Success" });
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




    }
}