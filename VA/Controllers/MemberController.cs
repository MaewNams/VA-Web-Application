using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;

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
            if (month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;

            }

            Member member = MemberService.GetByID(id);
            ViewData["Pets"] = PetService.GetByMemberID(id);
            ViewData["PetTypes"] = PetSpecieService.GetAll();
            ViewData["AppointmentsWait"] = AppointmentService.GetByMemberId(id, month.Value, year.Value, "Waiting");
      
            var dateTime = new DateTime(year.Value, month.Value, 1);
            ViewBag.DateTime = dateTime;
            ViewBag.Year = year.Value;
            ViewBag.Month = month.Value;
            return View(member);
        }
        public string GetRandomPassword()
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
        public ActionResult CreateMember(string name, string surname, string address, string phoneNumber)
        {
            string codeID ;
            Member result ;
            do
            {
                codeID = GetRandomPassword();
                result = MemberService.GetByCodeID(codeID);
            } while (result != null);
            Member member = new Member();
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phoneNumber;
            member.codeId = codeID;
            MemberService.Add(member);
            return Json(new { Result = "Success", newMemberId= member.id });
        }
        [HttpPost]
        public ActionResult CreateApp(int? memberID, int? petID, string detail, string suggestion, DateTime date)
        {
            Appointment appointment = new Appointment();
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
            Pet pet = new Pet();
            pet.memberId = Int32.Parse(memberID.ToString());
            pet.typeId = Int32.Parse(petType.ToString());
            pet.name = petName;
            PetService.Add(pet);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string name, string surname, string address, string phonenumber)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Json(new { Result = "Fail, Name is required" });
            }
            if (String.IsNullOrEmpty(surname))
            {
                return Json(new { Result = "Fail, Surname is required" });
            }
            if (String.IsNullOrEmpty(address))
            {
                return Json(new { Result = "Fail, Address is required" });
            }
            if (String.IsNullOrEmpty(phonenumber))
            {
                return Json(new { Result = "Fail, Phone number is required" });
            }

            Member member = MemberService.GetByID(id);
            member.name = name;
            member.surname = surname;
            member.address = address;
            member.phonenumber = phonenumber;
            MemberService.Update(member);
            return Json(new { Result = "Success" });
        }


        [HttpPost]
        public ActionResult DeletePet(int petid)
        {
            Pet pet = PetService.GetById(petid);
            PetService.Delete(pet);
            return Json(new { Result = "Success" });
        }




    }



}