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
    public class AppointmentController : Controller
    {
        private VAContext _db = new VAContext();
    //    private AdministratorRepository AdminService = new AdministratorRepository();
    //    private MemberRepository MemberService = new MemberRepository();
     //   private AppointmentRepository AppointmentService = new AppointmentRepository();
        // GET: Appointment

        private readonly IAppointmentRepository _AppRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly ITimeBlockRepository _TimeBlockRepo;
        private readonly IAppTimeRepository _AppTimeRepo;
        private readonly IServiceRepository _ServiceRepo;
        private readonly IVCRepository _VCRepo;

        public AppointmentController(IAppointmentRepository appRepository, IMemberRepository memberRepository,
    IVCRepository VCRepository, ITimeBlockRepository timeBlockRepository,
            IAppTimeRepository appTimeRepository, IServiceRepository serviceRepository)
        {
            _AppRepo = appRepository;
            _MemberRepo = memberRepository;
            _TimeBlockRepo = timeBlockRepository;
            _AppTimeRepo = appTimeRepository;
            _ServiceRepo = serviceRepository;
            _VCRepo = VCRepository;
        }

        public AppointmentController()
        {

        }




        ////      [ Appointment ]   CheckTimeslot --> Get warning message --> Create appoitnemt  --> Update time slot ///////////////////  

        [HttpPost]
        public ActionResult CheckTimeSlotStatus(int? serviceID,int petID, DateTime date, DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null || endTime == null)
            {
                return Json(new { Result = "Please select start time and end time for the appointment" });
            }
            if (petID == 0)
            {
                return Json(new { Result = "Please select pet profile" });
            }
            // Create slot of time of the appointment //
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

            DateTime checkStart, checkEnd;
            VAService service = _ServiceRepo.GetById(serviceID.Value);
            var warningmessage = new List<String>();
            Boolean requireConfirm = false;
            foreach (var hour in hours)
            {
                checkStart = new DateTime(date.Year, date.Month, date.Day,
                                   hour.Hour, hour.Minute, 0, startTime.Value.Kind);
                checkEnd = checkStart.AddHours(0.5);
                TimeBlock checkTimeBlock = _TimeBlockRepo.GetByTime(checkStart, checkEnd);

                // Service not surgery
                if (service.description != "Surgery")
                {
                    if (checkTimeBlock != null)
                    {
                        if (checkTimeBlock.status == "Busy") //Go to dicision 1 , Do you still want to create?
                        {
                            warningmessage.Add("You already have" + checkTimeBlock.numberofCase + " case between" + checkStart + "to" + checkEnd + '\n');
                            requireConfirm = true;
                        }
                        if (checkTimeBlock.status == "Full") // --> End 1 stop create process
                        {
                            return Json(new { Result = "You already have a surgery case between" + checkStart + "-" + checkEnd + " , please change appointment time" });
                        }
                    }
                }

                // Service is surgery --> End 2 stop create process
                else
                {
                    if (checkTimeBlock.status != "Free")
                    {
                        return Json(new { Result = " Surgery case require 'Free' time slot but you already have a case between" + checkStart + "-" + checkEnd + " , please change appointment time" });
                    }
                }
                TempData["warning"] = warningmessage;
            }

            if ((bool)requireConfirm)  //---> path for decision 1
            {
                GetWarningMessage();
                return Json(new { Result = "Confirm" });
            }
            else { 
                return Json(new { Result = "Success" }); }}

        private JsonResult GetWarningMessage()
        {
            if (TempData["warning"] != null)
            {
                var ls = (List<string>)TempData["warning"];
                string[][] newKeys = ls.Select(x => new string[] { x }).ToArray();
                string json = JsonConvert.SerializeObject(newKeys);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return Json("Confirm");
        }

        [HttpPost]
        public ActionResult CreateApp(int? memberID, int? petID, int? serviceID, string suggestion, DateTime date, DateTime startTime, DateTime endTime)
        {
            // 1 Create app//
            int lastID;
            /*List<Appointment> CheckApp = _AppRepo.GetAll().ToList();*/
            Appointment CheckApp = _AppRepo.GetLast();
            if (CheckApp != null)
            {
                lastID = CheckApp.id;
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
            appointment.startTime = startTime;
            appointment.endTime = endTime;
            appointment.status = "Waiting";
            _AppRepo.Add(appointment);

            // Call update time slot
            UpdateTimeSlot(serviceID, date, startTime, endTime, appointment);


            return Json(new { Result = "Success" });
        }

        private void UpdateTimeSlot(int? serviceID, DateTime date, DateTime startTime, DateTime endTime, Appointment appointment)
        {

            //Create object clinic to get maximum case
            Clinic clinic = _VCRepo.Get();
            VAService service = _ServiceRepo.GetById(serviceID.Value);

            //---> Create hour list of duration of appointment
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

            int lastAppTime; //--> use for create new id of new apppointmentTimeblock

            //  List<TimeBlock> CheckTime = TimeBlockService.GetAll().ToList();
            DateTime checkStart, checkEnd;
            var warningmessage = new List<String>();
            foreach (var hour in hours)
            {
                //---------------> Find timeblock object to add to AppointmentTimeBlock and add case number to timeblock
                checkStart = new DateTime(date.Year, date.Month, date.Day,
                                    hour.Hour, hour.Minute, 0, startTime.Kind);
                checkEnd = checkStart.AddHours(0.5);
                TimeBlock checkTimeBlock = _TimeBlockRepo.GetByTime(checkStart, checkEnd);


                //Check for last AppointmentTimeblock to get id
                AppointmentTimeBlock CheckAppTime = _AppTimeRepo.GetLast();
                if (CheckAppTime != null)
                { lastAppTime = CheckAppTime.id; }
                else
                { lastAppTime = 0; }

                //Create AppointmentTimeblock
                AppointmentTimeBlock appTime = new AppointmentTimeBlock();
                appTime.id = lastAppTime + 1;
                appTime.timeId = checkTimeBlock.id;
                appTime.appointmentID = appointment.id;
                _AppTimeRepo.Add(appTime);
                //--->Update timeblock object
                checkTimeBlock.numberofCase += 1;
                _TimeBlockRepo.Update(checkTimeBlock);

                // If case is not surgery
                if (service.description != "Surgery")
                {
                    // Still not fix the number of case that can take at a time
                    if (checkTimeBlock.numberofCase >= clinic.maximumCase)
                    {
                        checkTimeBlock.status = "Busy";
                        _TimeBlockRepo.Update(checkTimeBlock);
                    }
                }
                else
                {
                    checkTimeBlock.status = "Full";
                    _TimeBlockRepo.Update(checkTimeBlock);

                }
            }

        }



        [HttpPost]
        public ActionResult Edit(int? appid,  string suggestion)
        {
            Appointment appointment = _AppRepo.GetById(appid.Value);
            appointment.suggestion = suggestion;
            _AppRepo.Update(appointment);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult Delete(int appid)
        {
            Appointment appointment = _AppRepo.GetById(appid);
            _AppRepo.Delete(appointment);
            return Json(new { Result = "Success" });
        }
    }
}