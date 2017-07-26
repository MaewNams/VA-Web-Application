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
        private VAContext _db = new VAContext();

        private readonly IVCRepository _VCRepo;
        private readonly IPetSpecieRepository _SpecieRepo;
        private readonly IPetRepository _PetRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly IAppointmentRepository _AppRepo;
        private readonly IAdministratorRepository _AdminRepo;

        public HomeController(IAppointmentRepository appRepository, 
            IAdministratorRepository adminRepository, IMemberRepository memberRepository,
            IPetRepository petRepository, IPetSpecieRepository specieRepository,
            IVCRepository VCRepository)
        {
            _AppRepo = appRepository;
            _AdminRepo = adminRepository;
            _MemberRepo = memberRepository;
            _PetRepo = petRepository;
            _SpecieRepo = specieRepository;
            _VCRepo = VCRepository;
        }

        public HomeController()
        {

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
                AppWait = _AppRepo.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Waiting"),
                AppCom = _AppRepo.GetByDayAndMonthAndYear(day.Value, month.Value, year.Value, "Complete"),
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
                AppWait = _AppRepo.GetByMonthAndYear( month.Value, year.Value, "Waiting"),
                AppCom = _AppRepo.GetByMonthAndYear( month.Value, year.Value, "Complete"),
            };


            return View(result);
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
                return Json(new { Result = "Fail, maximum case can only numeric character" });
            }
            Clinic clinic = _VCRepo.Get();
            clinic.maximumCase = Int32.Parse(caseNumber.ToString());
            _VCRepo.Update(clinic);
            return Json(new { Result = "Success" });
        }

        public ActionResult PetSpecie()
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            IEnumerable<PetType> species= _SpecieRepo.GetAll();
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
            PetType type = _SpecieRepo.GetByName(name);
            if (type != null)
            {
                return Json(new { Result = "Fail, this specie is already exits in the system" });
            }

            List<PetType> CheckType = _SpecieRepo.GetAll().ToList();
            int lastID = CheckType.LastOrDefault().id;

            PetType newtype = new PetType();
            newtype.id = lastID + 1;
            newtype.name = name;
            _SpecieRepo.Add(newtype);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSpecie(string typeID, string name)
        {
            int id = Int32.Parse(typeID.ToString());

            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, specie name is required" });
            }
            PetType type = _SpecieRepo.GetByName(name);
            if (type != null && type.id != id)
            {
                return Json(new { Result = "Fail, this specie is already exits in the system" });
            }
            PetType editType = _SpecieRepo.GetById(id);
            editType.name = name;
            _SpecieRepo.Update(editType);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteSpecie(string typeID)
        {
            int id = Int32.Parse(typeID.ToString());
            Pet deletePetType = _PetRepo.GetByType(id);


            if (deletePetType != null)
            {
                return Json(new { Result = "Fail, cannot delete the pet specie. Please delete all pet which belong to the specie '" + deletePetType.PetType.name + "' before try again" });
            }
            /////////
            PetType deleteType = _SpecieRepo.GetById(id);
            _SpecieRepo.Delete(deleteType);
            return Json(new { Result = "Success" });
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



        public ActionResult Login()
        {
            if (Session["Authen"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Your Login page.";

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
                ViewBag.LoginError = "Username or password is in correct";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["Authen"] = null;
            Session["Username"] = null;
            Session["AccountID"] = null;

            return RedirectToAction("Login");

        }



    }
}