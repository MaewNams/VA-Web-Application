using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.ViewModel;
using VA.Repositories;
using VA.Service;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VA.Controllers
{
    public class HomeController : Controller
    {
   //     private VAContext _db = new VAContext();

        private readonly IVCRepository _VCRepo;
        private readonly ISpecieRepository _SpecieRepo;
        private readonly IPetRepository _PetRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly IAppointmentRepository _AppRepo;
        private readonly IAdministratorRepository _AdminRepo;
        private readonly IAppTimeRepository _AppTimeRepo;
        private readonly ITimeSlotRepository _TimeSlotRepo;

        public HomeController(IAppointmentRepository appRepository, 
            IAdministratorRepository adminRepository, IMemberRepository memberRepository,
            IPetRepository petRepository, ISpecieRepository specieRepository,
            IVCRepository VCRepository, IAppTimeRepository appTimeRepository, ITimeSlotRepository TimeSlotRepository)
        {
            _AppRepo = appRepository;
            _AdminRepo = adminRepository;
            _MemberRepo = memberRepository;
            _PetRepo = petRepository;
            _SpecieRepo = specieRepository;
            _VCRepo = VCRepository;
            _AppTimeRepo = appTimeRepository;
            _TimeSlotRepo = TimeSlotRepository;
        }

        //Use for test
        public HomeController()
        {

        }

        public ActionResult Login()
        {
            if (Session["Authen"] != null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "username,password")] Administrator administrator)
        {
            var Authentication = _AdminRepo.GetByUsernamrAndPassword(administrator.username, administrator.password);

            if (Authentication != null)
            {
                Session["Authen"] = true;
                Session["username"] = Authentication.username.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.LoginError = "Username or password is incorrect";
                return View();
            }
        }

        public ActionResult Index(int? day, int? month, int? year)
        {


            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if(day == null && month == null && year == null) {
            day = DateTime.Now.Day;
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            }

            AllAppointmentViewModel result = new AllAppointmentViewModel
            {
                date = new DateTime(year.Value, month.Value, day.Value),
                AppWait = _AppRepo.GetByDayAndMonthAndYearAndStatus(day.Value, month.Value, year.Value, "Waiting"),
                AppCom = _AppRepo.GetByDayAndMonthAndYearAndStatus(day.Value, month.Value, year.Value, "Complete"),
            };

            return View(result);
        }

        public ActionResult Monthly(int? month, int? year)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (month == null && year == null)
            {
          
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }


            AllAppointmentViewModel result = new AllAppointmentViewModel
            {
                date = new DateTime(year.Value, month.Value, DateTime.Now.Day),
                AppWait = _AppRepo.GetByMonthAndYearAndStatus( month.Value, year.Value, "Waiting"),
                AppCom = _AppRepo.GetByMonthAndYearAndStatus( month.Value, year.Value, "Complete"),
            };


            return View(result);
        }


        public ActionResult Member(string condition, string keyword)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            IEnumerable<Member> member = Enumerable.Empty<Member>();

            if (condition == null)
            {
                member = _MemberRepo.GetAll().OrderBy(m => m.id);
            }
            if (condition == "email")
            {
                member = _MemberRepo.GetByEmail(keyword);
            }

            if (condition == "name")
            {
                member = _MemberRepo.GetByName(keyword);
            }

            if (condition == "address")
            {
                member = _MemberRepo.GetByAddress(keyword);
            }
            if (condition == "phone")
            {
                member = _MemberRepo.GetByPhoneNumber(keyword);
            }

            return View(member);
        }

        public ActionResult PetSpecie()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            IEnumerable<Specie> species= _SpecieRepo.GetAll();
            return View(species);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddSpecie(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, specie name is required" });
            }
            Specie type = _SpecieRepo.GetByName(name);
            if (type != null)
            {
                return Json(new { Result = "Fail, this specie is already exits in the system" });
            }


            Specie newtype = new Specie();
            newtype.name = name;
            _SpecieRepo.Add(newtype);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSpecie(int specieId, string name)
        {

            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, specie name is required" });
            }
            Specie type = _SpecieRepo.GetByName(name);
            if (type != null && type.id != specieId)
            {
                return Json(new { Result = "Fail, specie "+ name +" is already exits in the system" });
            }
            Specie editType = _SpecieRepo.GetById(specieId);
            editType.name = name;
            _SpecieRepo.Update(editType);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteSpecie(int specieId)
        {
            Pet deletePetType = _PetRepo.GetBySpecie(specieId);


            if (deletePetType != null)
            {
                return Json(new { Result = "Fail, cannot delete the pet specie which have a pet. Please delete all pet which belong to the specie before try again" });
            }
            /////////
            Specie deleteType = _SpecieRepo.GetById(specieId);
            _SpecieRepo.Delete(deleteType);
            return Json(new { Result = "Success" });
        }


        public ActionResult VASetting()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Clinic clinic = _VCRepo.Get();

            return View(clinic);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VASetting(string caseNumber)
        {
            Boolean isNumber = Regex.IsMatch(caseNumber, @"^\d+$");
            if (isNumber == false)
            {
                return Json(new { Result = "Fail, maximum case can only contain numeric character" });
            }
            Clinic clinic = _VCRepo.Get();
            clinic.maximumCase = Int32.Parse(caseNumber.ToString());
            _VCRepo.Update(clinic);

            List<AppointmentTimeSlot> appTimeblock = _AppTimeRepo.GetAll().ToList();
            if (appTimeblock != null && appTimeblock.Count > 0)
            {
                foreach (var apptime in appTimeblock)
                {
                    //Update Timeblock (2)
                    TimeSlot updateTimeBlock = _TimeSlotRepo.GetByID(apptime.timeId);
                    int id = apptime.appointmentID;
      
                    // 2.2 update status
                    if (updateTimeBlock.numberofCase < clinic.maximumCase)
                    {
                   
                      
                        if (updateTimeBlock.numberofCase == 1 && updateTimeBlock.status == "Full")
                        {
                            // not up date --> still Full
                        }
                        else
                        {
                            updateTimeBlock.status = "Free";
                            _TimeSlotRepo.Update(updateTimeBlock);
                        }
                    }
                    else {

                        if (updateTimeBlock.numberofCase == 1 && updateTimeBlock.status == "Full")
                        {
                            // not up date --> still Full
                        }
                        else
                        {
                            updateTimeBlock.status = "Busy";
                            _TimeSlotRepo.Update(updateTimeBlock);
                        }
                    }

                }
            }


            return Json(new { Result = "Success" });
        }


        public ActionResult Logout()
        {
            Session["Authen"] = null;
            Session["username"] = null;
            Session["AccountID"] = null;

            return RedirectToAction("Login");

        }



    }
}