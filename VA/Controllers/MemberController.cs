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

namespace VA.Controllers
{
    public class MemberController : Controller
    {
        private VAContext _db = new VAContext();
        private PetRepository PetService = new PetRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        private MemberRepository MemberService = new MemberRepository();
        private PetSpecieRepository PetSpecieService = new PetSpecieRepository();
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
        public string GetRandomIDCode()
        {
            const string randomAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int alphabetAmount = 6;

            var result = "";

            for (var i = 1; i <= alphabetAmount; i++)
            {
                var characterPosition = _random.Next(0, randomAlphabet.Length - 1);
                var character = randomAlphabet[characterPosition];
                result += character;
            }

            return result;
        }

        [HttpPost]  
        public JsonResult GetRandomPassword()
        {
            const string randomAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int alphabetAmount = 6;

            var result = "";


            for (var i = 1; i <= alphabetAmount; i++)
            {
                var characterPosition = _random.Next(0, randomAlphabet.Length - 1);
                var character = randomAlphabet[characterPosition];
                result += character;
            }
            return Json(new { Result = "Success", newPassword = result});
        }

        [HttpPost]
        public ActionResult CreateMember(string name, string surname, string address, string phoneNumber)
        {
            Member checkName = MemberService.GetByNameAndSurname(name, surname);
            if(checkName != null)
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

            string codeID ;
            string password;
            Member result ;
            do
            {
                codeID = GetRandomIDCode();
                result = MemberService.GetByCodeIDAndPassword(codeID, "");
                password = GetRandomIDCode();
            } while (result != null);

            List<Member> CheckMem = MemberService.GetAll().ToList();
            int lastID = CheckMem.LastOrDefault().id;


            Member member = new Member();
            member.id = lastID+1;
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phoneNumber;
            member.codeId = codeID;
            member.password = password;
            MemberService.Add(member);
            return Json(new { Result = "Success", newMemberId= member.id });
        }
        [HttpPost]
        public ActionResult CreateApp(int? memberID, int? petID, string detail, string suggestion, DateTime date)
        {
            List<Appointment> CheckApp = AppointmentService.GetAll().ToList();
            int lastID = CheckApp.LastOrDefault().id;

            Appointment appointment = new Appointment();
            appointment.id = lastID + 1;
            appointment.memberId = Int32.Parse(memberID.ToString());
            appointment.petId = Int32.Parse(petID.ToString());
            appointment.detail = detail;
            appointment.suggestion = suggestion;
            appointment.date = date;
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
                return Json(new { Result = "Fail, member with name"+ name + "and surname " + surname + " already exits in the system" });
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
            if ( isNumber == false)
            {
                return Json(new { Result = "Fail, phone number can only contain 0-9" });
            }
            if(phonenumber.Length != 10)
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




    }



}