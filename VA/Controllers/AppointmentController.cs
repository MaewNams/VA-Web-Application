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
    public class AppointmentController : Controller
    {
        private VAContext _db = new VAContext();
        private AdministratorRepository AdminService = new AdministratorRepository();
        private MemberRepository MemberService = new MemberRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        // GET: Appointment
        [HttpPost]
        public ActionResult Delete(int appid)
        {
            Appointment appointment = AppointmentService.GetById(appid);
            AppointmentService.Delete(appointment);
            return Json(new { Result = "Success" });
        }
    }
}