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
        private VAContext _db = new VAContext();


        private readonly IAppointmentRepository _AppRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly IPetRepository _PetRepo;
        private readonly IPetSpecieRepository _SpecieRepo;
        private readonly ITimeBlockRepository _TimeBlockRepo;
        private readonly IAppTimeRepository _AppTimeRepo;
        private readonly IServiceRepository _ServiceRepo;
        private readonly IVCRepository _VCRepo;

        public MemberController(IAppointmentRepository appRepository, IMemberRepository memberRepository,
    IPetRepository petRepository, IPetSpecieRepository specieRepository,
    IVCRepository VCRepository, ITimeBlockRepository timeBlockRepository,
            IAppTimeRepository appTimeRepository, IServiceRepository serviceRepository)
        {
            _AppRepo = appRepository;
            _MemberRepo = memberRepository;
            _PetRepo = petRepository;
            _SpecieRepo = specieRepository;
            _VCRepo = VCRepository;
            _TimeBlockRepo = timeBlockRepository;
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

        public ActionResult MemberPet(int? id)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Member", "Home");
            }
            MemberPetViewModel memberPet = new MemberPetViewModel
            {
                member = _MemberRepo.GetByID(id.Value),
                pets = _PetRepo.GetByMemberID(id.Value),
                pettypes = _SpecieRepo.GetAll()
            };

            return View(memberPet);
        }


        public ActionResult CreateAppointment(int? id, int? day, int? month, int? year)
        {
            if (Session["Authen"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            Member member = _MemberRepo.GetByID(id.Value);
            if (member == null)
            {
                return RedirectToAction("Member", "Home");
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
                member = _MemberRepo.GetByID(id.Value),
                pets = _PetRepo.GetByMemberID(id.Value),
                services = _ServiceRepo.GetAll(),
                timeblocks = _TimeBlockRepo.GetListByDate(day.Value, month.Value, year.Value)
            };

            return View(result);
        }

        //// [ Member ]  Create - Edit - Reset password - Delete  ///////////////////   
        [HttpPost]
        public ActionResult CreateMember(string name, string surname, string address, string email, string phoneNumber)
        {
            Member checkName = _MemberRepo.GetByNameAndSurname(name, surname);
            Member checkEMail = _MemberRepo.GetByEmail(email).FirstOrDefault();
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
                return Json(new { Result = "Fail, email address is in invalid format" });
            }
            if (checkEMail != null)
            {
                return Json(new { Result = "Fail, the email " + email + " already exits in the system" });
            }
            string password = name[0].ToString().ToUpper() + phoneNumber;


            Member CheckMem = _MemberRepo.GetLast();
            int lastID;
            if (CheckMem == null)
            {
                lastID = 0;
            }
            else
            {
                lastID = CheckMem.id;
            }

            Member member = new Member();
            member.id = lastID + 1;
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
            Member checkEMail = _MemberRepo.GetByEmail(email).FirstOrDefault();
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
                return Json(new { Result = "Fail, email address is in invalid format" });
            }
            if (checkEMail != null && checkEMail.id != id)
            {
                return Json(new { Result = "Fail, the email " + email + " already exits in the system" });
            }

            Member member = _MemberRepo.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
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
            ViewData["DeleteAppointment"] = _db.Appointment.Where(a => a.memberId == memberid).ToList<Appointment>();
            List<Appointment> memApp = (List<Appointment>)ViewData["DeleteAppointment"];
            if (memApp != null && memApp.Count > 0)
            {
                foreach (var app in memApp)
                {   /*But first, remove appointment from time slot*/
                    // Check for time slot of each appointment
                    List<AppointmentTimeBlock> appTimeblock = _AppTimeRepo.GetByAppointmentID(app.id).ToList();
                    if (appTimeblock != null && appTimeblock.Count > 0)
                    {
                        foreach (var apptime in appTimeblock)
                        {
                            //Update Timeblock (2)
                            TimeBlock updateTimeBlock = _TimeBlockRepo.GetByID(apptime.timeId);
                            // 2.1 reduce number of case
                            updateTimeBlock.numberofCase -= 1;

                            // 2.2 update status
                            if (updateTimeBlock.numberofCase == 0 || updateTimeBlock.numberofCase <= clinic.maximumCase)
                            {
                                updateTimeBlock.status = "Free";
                            }

                            _TimeBlockRepo.Update(updateTimeBlock);

                            //Delete AppointmentTimeblock (1) ^
                            _AppTimeRepo.Delete(apptime);
                        }
                    }
                    // Then remove appointment (3)
                    _db.Appointment.Remove(app);
                    _db.SaveChanges();
                }
            }

            //Then Remove pet (4)
            ViewData["DeletePet"] = _db.Pet.Where(a => a.memberId == memberid).ToList<Pet>();
            List<Pet> memPet = (List<Pet>)ViewData["DeletePet"];
            if (memPet != null && memPet.Count > 0)
            {
                foreach (var pet in memPet)
                {
                    _db.Pet.Remove(pet);
                    _db.SaveChanges();
                }
            }

            Member member = _MemberRepo.GetByID(memberid);
            _MemberRepo.Delete(member);



            return Json(new { Result = "Success" });
        }

        ////      [ Pet ]   Create - Edit - Delete  ///////////////////  

        [HttpPost]
        public ActionResult CreatePet(int? memberID, int? petType, string petName)
        {

            Pet Checkname = _PetRepo.GetByMemberIDAndNameAndSpecie(memberID.Value, petName, petType.Value);
            if (Checkname != null)
            {
                return Json(new { Result = "Fail, The member already have " + (_SpecieRepo.GetById(petType.Value)).name + " name " + petName });
            }

            Pet CheckPetID = _PetRepo.GetLast();
            int lastID;
            if (CheckPetID == null)
            {
                lastID = 0;
            }
            else
            {
                lastID = CheckPetID.id;
            }

            Pet pet = new Pet();
            pet.id = lastID + 1;
            pet.memberId = Int32.Parse(memberID.ToString());
            pet.typeId = Int32.Parse(petType.ToString());
            pet.name = petName;
            _PetRepo.Add(pet);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult EditPet(int? petID, int? petType, string petName)
        {
            if (String.IsNullOrEmpty(petName))
            {
                return Json(new { Result = "Fail, name is required" });
            }
            Pet pet = _PetRepo.GetById(petID.Value);
            //   Debug.WriteLine(pet.name + "first");

            pet.name = petName;
            pet.typeId = petType.Value;
            _PetRepo.Update(pet);
            //    Debug.WriteLine(pet.name + "second");

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
                    _db.Appointment.Remove(app);
                    _db.SaveChanges();
                }
            }

            _PetRepo.Delete(pet);
            return Json(new { Result = "Success" });
        }


        /////////////   Check tim slot to create if not exits

        private void CheckExitsTimeSlot(int? day, int? month, int? year)
        {
            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 30, 0);
            DateTime today = new DateTime(year.Value, month.Value, day.Value);
            // Check if the system already have that day time table -- if not, create one --of the week//

            TimeBlock checkExits = _TimeBlockRepo.GetByDate(day.Value, month.Value, year.Value);
            var monday = today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            if (checkExits == null)
            {
                int lastDay = monday.Day + 7;
                //Loop for this week
                for (int i = monday.Day - 1; i <= lastDay; i++)
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
                    TimeBlock checkLast = _TimeBlockRepo.GetLast();
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
                        _TimeBlockRepo.Add(timeblock);
                        lastTimeID += 1;
                    }
                }
            }
        }


    }



}