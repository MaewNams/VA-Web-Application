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
        public ActionResult Edit(int? appid, string detail, string suggestion)
        {
            if (String.IsNullOrEmpty(detail))
            {
                return Json(new { Result = "Fail, appointment detail is required" });
            }

            Appointment appointment = AppointmentService.GetById(appid.Value);
            appointment.detail = detail;
            appointment.suggestion = suggestion;
            AppointmentService.Update(appointment);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult Delete(int appid)
        {
            Appointment appointment = AppointmentService.GetById(appid);
            AppointmentService.Delete(appointment);
            return Json(new { Result = "Success" });
        }
    }
}