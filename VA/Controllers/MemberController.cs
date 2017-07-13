using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VA.Controllers
{
    public class MemberController : Controller
    {
        private VAContext _db = new VAContext();
        private PetRepository PetService = new PetRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private PetSpecieRepository PetSpecieService = new PetSpecieRepository();
        private TimeBlockRepository TimeBlockService = new TimeBlockRepository();
        private AppTimeRepository AppTimeService = new AppTimeRepository();
        private VAServiceRepository VAService = new VAServiceRepository();
        private VCRepository VCService = new VCRepository();
        private readonly Random _random = new Random();

        //[Route("Member/{Id}")]
        public ActionResult Index(int id, int? month, int? year)
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

            Member member = MemberService.GetByID(id);
            ViewData["Pets"] = PetService.GetByMemberID(id);
            ViewData["PetTypes"] = PetSpecieService.GetAll();
            ViewData["AppointmentsWait"] = AppointmentService.GetByMemberIdAndStatus(id, month.Value, year.Value, "Waiting");
            ViewData["AppointmentsCom"] = AppointmentService.GetByMemberIdAndStatus(id, month.Value, year.Value, "Complete");

            var dateTime = new DateTime(year.Value, month.Value, 1);
            ViewBag.DateTime = dateTime;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            return View(member);
        }

        [HttpPost]
        public ActionResult CreateMember(string name, string surname, string address, string email, string phoneNumber)
        {
            Member checkName = MemberService.GetByNameAndSurname(name, surname);
            if (checkName != null)
            {
                return Json(new { Result = "Fail, member with name" + name + "and surname " + surname + " already exits in the system" });
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

            string password = name[0].ToString().ToUpper() + phoneNumber;


            List<Member> CheckMem = MemberService.GetAll().ToList();
            int lastID = CheckMem.LastOrDefault().id;


            Member member = new Member();
            member.id = lastID + 1;
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phoneNumber;
            member.email = email;
            member.password = password;
            MemberService.Add(member);
            return Json(new { Result = "Success", newMemberId = member.id });
        }
        [HttpPost]
        public ActionResult CreateApp(int? memberID, int? petID, int? serviceID, string suggestion, DateTime date, DateTime startTime, DateTime endTime)
        {
            List<Appointment> CheckApp = AppointmentService.GetAll().ToList();
            int lastID = CheckApp.LastOrDefault().id;

            Appointment appointment = new Appointment();
            appointment.id = lastID + 1;
            appointment.memberId = Int32.Parse(memberID.ToString());
            appointment.petId = Int32.Parse(petID.ToString());
            appointment.serviceId = Int32.Parse(serviceID.ToString());
            appointment.suggestion = suggestion;
            appointment.date = date;
            appointment.startTime = startTime;
            appointment.endTime = endTime;
            appointment.status = "Waiting";
            AppointmentService.Add(appointment);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult CreatePet(int? memberID, int? petType, string petName)
        {
            List<Pet> CheckPetID = PetService.GetAll().ToList();
            int lastID = CheckPetID.LastOrDefault().id;
            List<Pet> CheckName = PetService.GetByMemberID(memberID.Value).ToList();
            Pet Checkname = PetService.GetByMemberIDAndNameAndSpecie(memberID.Value, petName, petType.Value);
            if (Checkname != null)
            {
                return Json(new { Result = "Fail, The member already have " + (PetSpecieService.GetById(petType.Value)).name + " name " + petName });
            }

            Pet pet = new Pet();
            pet.id = lastID + 1;
            pet.memberId = Int32.Parse(memberID.ToString());
            pet.typeId = Int32.Parse(petType.ToString());
            pet.name = petName;
            PetService.Add(pet);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult EditPet(int? petID, int? petType, string petName)
        {
            if (String.IsNullOrEmpty(petName))
            {
                return Json(new { Result = "Fail, name is required" });
            }
            Pet pet = PetService.GetById(petID.Value);
            Debug.WriteLine(pet.name + "first");

            pet.name = petName;
            pet.typeId = petType.Value;
            PetService.Update(pet);
            Debug.WriteLine(pet.name + "second");

            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string name, string surname, string address, string phonenumber, string password)
        {
            Member checkName = MemberService.GetByNameAndSurname(name, surname);
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

            Member member = MemberService.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phonenumber;
            member.password = password;
            MemberService.Update(member);
            return Json(new { Result = "Success" });
        }
        [HttpPost]
        public ActionResult ResetPassword(int memberid)
        {
            Member member = MemberService.GetByID(memberid);
            member.password = member.name[0].ToString().ToUpper() + member.phonenumber;
            MemberService.Update(member);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult DeletePet(int petid)
        {
            Pet pet = PetService.GetById(petid);
            List<Appointment> appointment = AppointmentService.GetByPetID(petid).ToList();
            if (appointment != null && appointment.Count > 0)
            {
                foreach (var app in appointment)
                {
                    _db.Appointment.Remove(app);
                    _db.SaveChanges();
                }
            }

            PetService.Delete(pet);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult Delete(int memberid)
        {
            /*delte appointment first*/
            ViewData["DeleteAppointment"] = _db.Appointment.Where(a => a.memberId == memberid).ToList<Appointment>();
            List<Appointment> memApp = (List<Appointment>)ViewData["DeleteAppointment"];
            if (memApp != null && memApp.Count > 0)
            {
                foreach (var app in memApp)
                {
                    _db.Appointment.Remove(app);
                    _db.SaveChanges();
                }
            }

            //Remove pet
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

            Member member = MemberService.GetByID(memberid);
            MemberService.Delete(member);
            return Json(new { Result = "Success" });
        }





        /*Test*/
        //[Route("Member/{Id}")]
        public ActionResult TestCreateApp(int id, int? day, int? month, int? year)
        {
            ViewData["warning"] = null;
            if (TempData["warning"] != null)
            {
                var ls = (List<string>)TempData["warning"];
                foreach (string str in ls)
                {

                    ViewData["warning"] = str;


                }
            }

            /* if (Session["Authen"] == null)
             {
                 return RedirectToAction("Login", "Home");
             }*/

            if (month == null && year == null)
            {
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;

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

            ///

            Member member = MemberService.GetByID(id);
            ViewData["Pets"] = PetService.GetByMemberID(id);
            ViewData["PetTypes"] = PetSpecieService.GetAll();
            ViewData["Services"] = VAService.GetAll();
            ViewData["TimeBlock"] = TimeBlockService.GetListByDate(day.Value, month.Value, year.Value);
            ViewData["AllApp"] = AppointmentService.GetByDayAndMonthAndYearNoStatus(id, day.Value, month.Value, year.Value);
            ViewData["AppointmentsWait"] = AppointmentService.GetByMemberIdAndStatus(id, month.Value, year.Value, "Waiting");
            ViewData["AppointmentsCom"] = AppointmentService.GetByMemberIdAndStatus(id, month.Value, year.Value, "Complete");

            var dateTime = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.DateTime = dateTime;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            return View(member);
        }

        [HttpPost]
        public ActionResult CheckTimeSlot(int? serviceID, DateTime date, DateTime? startTime, DateTime? endTime)
        {
            // Create slot of time //
            var hours = new List<DateTime>();

            var start = new DateTime(date.Year, date.Month, date.Day,
                                    startTime.Value.Hour, startTime.Value.Minute, 0, startTime.Value.Kind);
            var end = new DateTime(date.Year, date.Month, date.Day,
                                    endTime.Value.Hour, endTime.Value.Minute, 0, endTime.Value.Kind);
            hours.Add(start);

            while ((start = start.AddHours(0.5)) < end)
            {
                hours.Add(start);
            }

            List<TimeBlock> CheckTime = TimeBlockService.GetAll().ToList();
            DateTime checkStart, checkEnd;
            VAService service = VAService.GetById(serviceID.Value);
            var warningmessage = new List<String>();
            TempData.Remove("warning");
            ViewData["RequireConfirmation"] = false;
            foreach (var hour in hours)
            {
                checkStart = new DateTime(date.Year, date.Month, date.Day,
                                   hour.Hour, hour.Minute, 0, startTime.Value.Kind);
                checkEnd = checkStart.AddHours(0.5);
                TimeBlock checkTimeBlock = TimeBlockService.GetByTime(checkStart, checkEnd);

                // Service not surgery
                if (service.description != "Surgery")
                {
                    if (checkTimeBlock != null)
                    {
                        if (checkTimeBlock.status == "Busy")
                        {
                            warningmessage.Add("You already have" + checkTimeBlock.numberofCase + " case between" + checkStart + "to" + checkEnd + '\n');
                            ViewData["RequireConfirmation"] = true;
                        }
                        if (checkTimeBlock.status == "Full")
                        {
                            return Json(new { Result = "You already have a surgery case between" + checkStart + "-" + checkEnd + " , please change appointment time" });
                        }
                    }
                }

                // Service is surgery
                else
                {
                    if (checkTimeBlock.status != "Free")
                    {
                        return Json(new { Result = " Surgery case require 'Free' time slot but you already have a case between" + checkStart + "-" + checkEnd + " , please change appointment time" });
                    }
                }
                TempData["warning"] = warningmessage;
                /*    Console.WriteLine(warningmessage);*/
            }

            if ((bool)ViewData["RequireConfirmation"])
            {
                return Json(new { Result = "Confirm" });
            }
            else { return Json(new { Result = "Success" }); }

        }

        public JsonResult GetWarningMessage()
        {
            if (TempData["warning"] != null)
            {
                var ls = (List<string>)TempData["warning"];
                string[][] newKeys = ls.Select(x => new string[] { x }).ToArray();
                string json = JsonConvert.SerializeObject(newKeys);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return Json("OK");
        }

        [HttpPost]
        public ActionResult TestCreateApp(int? memberID, int? petID, int? serviceID, string suggestion, DateTime date, DateTime startTime, DateTime endTime)
        {

            VAService service = VAService.GetById(serviceID.Value);
            //Create app//
            int lastID;
            List<Appointment> CheckApp = AppointmentService.GetAll().ToList();
            if (CheckApp.Count() > 0)
            {
                lastID = CheckApp.LastOrDefault().id;
            }
            else
            {
                lastID = 0;
            }

       
            Appointment appointment = new Appointment();
            appointment.id = lastID + 1;
            appointment.memberId = Int32.Parse(memberID.ToString());
            appointment.petId = Int32.Parse(petID.ToString());
            appointment.serviceId = Int32.Parse(serviceID.ToString());
            appointment.suggestion = suggestion;
            appointment.date = date;
            /*     if (type == "allDay")
                 {
                     startTime = new DateTime(1900,01,01);
                     endTime = new DateTime(1900, 01, 01);
                 }*/

            //Create object clinic to get maximum case
            Clinic clinic = VCService.Get();
           appointment.startTime = startTime;
            appointment.endTime = endTime;
            appointment.status = "Waiting";
            AppointmentService.Add(appointment);

                var hours = new List<DateTime>();

                var start = new DateTime(date.Year, date.Month, date.Day,
                                        startTime.Hour, startTime.Minute, 0, startTime.Kind);
                var end = new DateTime(date.Year, date.Month, date.Day,
                                        endTime.Hour, endTime.Minute, 0, startTime.Kind);
                hours.Add(start);

                while ((start = start.AddHours(0.5)) < end)
                {
                    hours.Add(start);
                }
                int lastAppTime;
                List<TimeBlock> CheckTime = TimeBlockService.GetAll().ToList();
                DateTime checkStart, checkEnd;
                var warningmessage = new List<String>();
                foreach (var hour in hours)
                {
                    checkStart = new DateTime(date.Year, date.Month, date.Day,
                                        hour.Hour, hour.Minute, 0, startTime.Kind);
                    checkEnd = checkStart.AddHours(0.5);

                    TimeBlock checkTimeBlock = TimeBlockService.GetByTime(checkStart, checkEnd);

                    List<AppointmentTimeBlock> CheckAppTime = AppTimeService.GetAll().ToList();
                    if (CheckAppTime.Count() > 0)
                    {
                        lastAppTime = CheckAppTime.LastOrDefault().id;
                    }
                    else
                    {
                        lastAppTime = 0;
                    }

                    //Create appointment
                    AppointmentTimeBlock appTime = new AppointmentTimeBlock();
                    appTime.id = lastAppTime + 1;
                    appTime.timeId = checkTimeBlock.id;
                    appTime.appointmentID = appointment.id;
                    checkTimeBlock.numberofCase += 1;
                    TimeBlockService.Update(checkTimeBlock);

                    // If case is not surgery
                    if (service.description != "Surgery")
                    {
                        // Still not fix the number of case that can take at a time
                        if (checkTimeBlock.numberofCase > clinic.maximumCase)
                        {
                            checkTimeBlock.status = "Busy";
                            TimeBlockService.Update(checkTimeBlock);
                        }
                    }
                    else
                    {
                        checkTimeBlock.status = "Full";

                    }
                }
            

            return Json(new { Result = "Success" });
        }

    }

}