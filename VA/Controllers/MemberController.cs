using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.ViewModel;
using VA.Repositories;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VA.Controllers
{
    public class MemberController : Controller
    {
       // private VAContext _db = new VAContext();


        private readonly IAppointmentRepository _AppRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly IPetRepository _PetRepo;
        private readonly ISpecieRepository _SpecieRepo;
        private readonly ITimeSlotRepository _TimeSlotRepo;
        private readonly IAppTimeRepository _AppTimeRepo;
        private readonly IServiceRepository _ServiceRepo;
        private readonly IVCRepository _VCRepo;

        public MemberController(IAppointmentRepository appRepository, IMemberRepository memberRepository,
    IPetRepository petRepository, ISpecieRepository specieRepository,
    IVCRepository VCRepository, ITimeSlotRepository timeBlockRepository,
            IAppTimeRepository appTimeRepository, IServiceRepository serviceRepository)
        {
            _AppRepo = appRepository;
            _MemberRepo = memberRepository;
            _PetRepo = petRepository;
            _SpecieRepo = specieRepository;
            _VCRepo = VCRepository;
            _TimeSlotRepo = timeBlockRepository;
            _AppTimeRepo = appTimeRepository;
            _ServiceRepo = serviceRepository;
        }

        public MemberController()
        {

        }

        public ActionResult Index(int id, int? month, int? year)
        {
            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            MemberAppointmentViewModel memberApp = new MemberAppointmentViewModel
            {
                date = new DateTime(year.Value, month.Value, DateTime.Now.Day),
                member = _MemberRepo.GetByID(id),
                waitAppointments = _AppRepo.GetByMemberIdWithMothAndYearAndStatus(id, month.Value, year.Value, "Waiting").ToList(),
                comAppointments = _AppRepo.GetByMemberIdWithMothAndYearAndStatus(id, month.Value, year.Value, "Complete").ToList()

            };
            return View(memberApp);
        }
         
        public ActionResult MemberPet(int id)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            MemberPetViewModel memberPet = new MemberPetViewModel
            {
                member = _MemberRepo.GetByID(id),
                pets = _PetRepo.GetByMemberID(id),
                species = _SpecieRepo.GetAll()
            };

            return View(memberPet);
        }   


        public ActionResult CreateAppointment(int id, int? day, int? month, int? year)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            if (month == null && year == null)
            {
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            CheckExitsTimeSlot(day, month, year);

            CreateAppointmentViewModel result = new CreateAppointmentViewModel
            {
                date = new DateTime(year.Value, month.Value, day.Value),
                member = _MemberRepo.GetByID(id),
                pets = _PetRepo.GetByMemberID(id),
                services = _ServiceRepo.GetAll(),
                timeblocks = _TimeSlotRepo.GetListByDate(day.Value, month.Value, year.Value)
            };

            return View(result);
        }

        //// [ Member ]  Create - Edit - Reset password - Delete  ///////////////////   
        [HttpPost]
        public ActionResult CreateMember(string name, string surname, string address, string email, string phoneNumber)
        {
            Member checkName = _MemberRepo.GetByNameAndSurname(name, surname);
            Member checkEMail = _MemberRepo.GetByExactlyEmail(email);
            var foo = new EmailAddressAttribute();
            bool bar = foo.IsValid(email);
            if (checkName != null)
            {
                return Json(new { Result = "Fail, member with name " + name + "and surname " + surname + " already exits in the system" });
            }
            Boolean isNumber = Regex.IsMatch(phoneNumber, @"^\d+$");
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, name is required" });
            }
            if (String.IsNullOrEmpty(surname))
            {
                return Json(new { Result = "Fail, surname is required" });
            }
            if (String.IsNullOrEmpty(email))
            {
                return Json(new { Result = "Fail, email address is required" });
            }
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { Result = "Fail, address is required" });
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
            if (bar == false)
            {
                return Json(new { Result = "Fail, email address is in invalid format, the valid format of email is xxx@xxx.xx" });
            }
            if (checkEMail != null)
            {
                return Json(new { Result = "Fail, the email already exits in the system" });
            }
            string password = name[0].ToString().ToUpper() + phoneNumber;


            Member member = new Member();
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phoneNumber;
            member.email = email;
            member.password = password;
            _MemberRepo.Add(member);
            return Json(new { Result = "Success", newMemberId = member.id });
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string name, string surname, string email, string address, string phonenumber)
        {
            Member checkName = _MemberRepo.GetByNameAndSurname(name, surname);
            Member checkEMail = _MemberRepo.GetByExactlyEmail(email);
            var foo = new EmailAddressAttribute();
            bool bar = foo.IsValid(email);
            if (checkName != null && checkName.id != id)
            {
                return Json(new { Result = "Fail, member with name" + name + "and surname " + surname + " already exits in the system" });
            }

            Boolean isNumber = Regex.IsMatch(phonenumber, @"^\d+$");
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, name is required" });
            }
            if (String.IsNullOrEmpty(surname))
            {
                return Json(new { Result = "Fail, surname is required" });
            }
            if (String.IsNullOrEmpty(email))
            {
                return Json(new { Result = "Fail, email address is required" });
            }
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { Result = "Fail, address is required" });
            }
            if (String.IsNullOrEmpty(phonenumber))
            {
                return Json(new { Result = "Fail, phone number is required" });
            }
            if (isNumber == false)
            {
                return Json(new { Result = "Fail, phone number can only contain 0-9" });
            }
            if (phonenumber.Length != 10)
            {
                return Json(new { Result = "Fail, phone number have to contain 10 numeric character" });
            }
            if (bar == false)
            {
                return Json(new { Result = "Fail, email address is in invalid format, the valid format of email is xxx@xxx.xx" });
            }
            if (checkEMail != null && checkEMail.id != id)
            {
                return Json(new { Result = "Fail, the email already exits in the system" });
            }

            Member member = _MemberRepo.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.email = email;
            member.phonenumber = phonenumber;
            _MemberRepo.Update(member);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult ResetPassword(int memberid)
        {
            Member member = _MemberRepo.GetByID(memberid);
            member.password = member.name[0].ToString().ToUpper() + member.phonenumber;
            _MemberRepo.Update(member);
            return Json(new { Result = "Success" });
        }


        [HttpPost]
        public ActionResult Delete(int memberid)
        {
            // Clinic object
            Clinic clinic = _VCRepo.Get();

            /*delte appointment first*/
            IEnumerable<Appointment> memApp = _AppRepo.GetByMemberId(memberid);
            if (memApp != null && memApp.Count() > 0)
            {
                foreach (var app in memApp)
                {   /*But first, remove appointment from time slot*/
                    // Check for time slot of each appointment
                    IEnumerable<AppointmentTimeSlot> appTimeblock = _AppTimeRepo.GetByAppointmentID(app.id);
                    if (appTimeblock != null && appTimeblock.Count() > 0)
                    {
                        foreach (var apptime in appTimeblock)
                        {
                            //Update Timeblock (2)
                            TimeSlot updateTimeBlock = _TimeSlotRepo.GetByID(apptime.timeId);
                            // 2.1 reduce number of case
                            updateTimeBlock.numberofCase -= 1;

                            // 2.2 update status
                            if (updateTimeBlock.numberofCase == 0 || updateTimeBlock.numberofCase <= clinic.maximumCase)
                            {
                                updateTimeBlock.status = "Free";
                            }

                            _TimeSlotRepo.Update(updateTimeBlock);

                            //Delete AppointmentTimeblock (1) ^
                            _AppTimeRepo.Delete(apptime);
                        }
                    }
                    // Then remove appointment (3)
                    _AppRepo.Delete(app);

                }
            }

            //Then Remove pet (4)
            IEnumerable<Pet> memPet = _PetRepo.GetByMemberID(memberid);
            if (memPet != null && memPet.Count() > 0)
            {
                foreach (var pet in memPet)
                {
                    _PetRepo.Delete(pet);
                }
            }

            Member member = _MemberRepo.GetByID(memberid);
            _MemberRepo.Delete(member);



            return Json(new { Result = "Success" });
        }

        ////      [ Pet ]   Create - Edit - Delete  ///////////////////  

        [HttpPost]
        public ActionResult CreatePet(int memberID, int specieID, string petName)
        {

            if(specieID == 0)
            {
                return Json(new { Result = "Fail, pet specie is required " });
            }
            if (String.IsNullOrEmpty(petName))
            {
                return Json(new { Result = "Fail, name is required" });
            }
            Pet Checkname = _PetRepo.GetByMemberIDAndNameAndSpecie(memberID, petName, specieID);
            if (Checkname != null)
            {
                return Json(new { Result = "Fail, The member already have " + (_SpecieRepo.GetById(specieID)).name + " name " + petName });
            }

            Pet pet = new Pet();
            pet.memberId = memberID;
            pet.specieId = specieID;
            pet.name = petName;
            _PetRepo.Add(pet);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult EditPet(int petID, int specieID, string petName)
        {
            if (String.IsNullOrEmpty(petName))
            {
                return Json(new { Result = "Fail, name is required" });
            }

            Pet pet = _PetRepo.GetById(petID);

            Pet Checkname = _PetRepo.GetByMemberIDAndNameAndSpecie(pet.memberId, petName, specieID);
            if (Checkname != null)
            {
                return Json(new { Result = "Fail, The member already have " + (_SpecieRepo.GetById(specieID)).name + " name " + petName });
            }
      

            pet.name = petName;
            pet.specieId = specieID;
            _PetRepo.Update(pet);

            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult DeletePet(int petid)
        {
            Pet pet = _PetRepo.GetById(petid);
            List<Appointment> appointment = _AppRepo.GetByPetID(petid).ToList();
            if (appointment != null && appointment.Count > 0)
            {
                foreach (var app in appointment)
                {
                    _AppRepo.Delete(app);
                //    _db.SaveChanges();
                }
            }

            _PetRepo.Delete(pet);
            return Json(new { Result = "Success" });
        }


        /////////////   Check tim slot to create if not exits

        private void CheckExitsTimeSlot(int? day, int? month, int? year)
        {
            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 00, 0);
            DateTime today = new DateTime(year.Value, month.Value, day.Value);
            // Check if the system already have that day time table -- if not, create one --of the week//
            DateTime lastMonday;
            DateTime endWeek;
            TimeSlot checkExits = _TimeSlotRepo.GetByDate(day.Value, month.Value, year.Value);

            if(today.DayOfWeek == DayOfWeek.Sunday) { 
            DateTime nextSunday = today.AddDays(7 - (int)today.DayOfWeek);
             lastMonday = nextSunday.AddDays(-13);
                 endWeek = lastMonday.AddDays(6);
            }
            else
            {

                int offset = today.DayOfWeek - DayOfWeek.Monday;
                 lastMonday = today.AddDays(-offset);
                endWeek = lastMonday.AddDays(6);
            }
         
        


            //  var monday = today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

           // int lastDay = lastMonday.Day + 6;
          //  var monday = today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
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
        }


    }



}