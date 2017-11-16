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
        // private VAContext _db = new VAContext();
        //    private AdministratorRepository AdminService = new AdministratorRepository();
        private MemberRepository MemberService = new MemberRepository();
        private AppointmentRepository AppointmentService = new AppointmentRepository();
        // GET: Appointment

        private readonly IAppointmentRepository _AppRepo;
        private readonly IMemberRepository _MemberRepo;
        private readonly ITimeSlotRepository _TimeSlotRepo;
        private readonly IAppTimeRepository _AppTimeRepo;
        private readonly IServiceRepository _ServiceRepo;
        private readonly IVCRepository _VCRepo;

        public AppointmentController(IAppointmentRepository appRepository, IMemberRepository memberRepository,
    IVCRepository VCRepository, ITimeSlotRepository timeBlockRepository,
            IAppTimeRepository appTimeRepository, IServiceRepository serviceRepository)
        {
            _AppRepo = appRepository;
            _MemberRepo = memberRepository;
            _TimeSlotRepo = timeBlockRepository;
            _AppTimeRepo = appTimeRepository;
            _ServiceRepo = serviceRepository;
            _VCRepo = VCRepository;
        }

        public AppointmentController()
        {

        }




        ////      [ Appointment ]   CheckTimeslot --> Get warning message --> Create appoitnemt  --> Update time slot ///////////////////  

        [HttpPost]
        public ActionResult CheckForCreateApp(int serviceID, int petID, DateTime date, DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null || endTime == null)
            {
                return Json(new { Result = "Fail, start time and end time is require" });
            }
            if (petID == 0)
            {
                return Json(new { Result = "Fail, pet is require" });
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
            VAService service = _ServiceRepo.GetById(serviceID);
            var warningmessage = new List<String>();
            TempData["warning"] = false;
            Boolean requireConfirm = false;
            foreach (var hour in hours)
            {
                checkStart = new DateTime(date.Year, date.Month, date.Day,
                                   hour.Hour, hour.Minute, 0, startTime.Value.Kind);
                checkEnd = checkStart.AddHours(0.5);
                TimeSlot checkTimeBlock = _TimeSlotRepo.GetByTime(checkStart, checkEnd);

                // Service not surgery
                if (service.description != "Surgery")
                {
                    if (checkTimeBlock != null)
                    {
                        if (checkTimeBlock.status == "Busy") //Go to dicision 1 , Do you still want to create?
                        {
                            requireConfirm = true;
                        }
                        if (checkTimeBlock.status == "Full") // --> End 1 stop create process
                        {
                         //   TempData["warning"] = true;
                            return Json(new { Result = "You already have a surgery case between" + startTime.Value.ToShortTimeString() + "-" + endTime.Value.ToShortTimeString() + " , please change appointment time" });
                        }
                    }
                }

                // Service is surgery --> End 2 stop create process
                else
                {
                    if (checkTimeBlock.numberofCase != 0)
                    {
                       // TempData["warning"] = true;
                        return Json(new { Result = "Fail, Surgery case require 'Free' time slot with 0 case, but you already have a case between" + startTime.Value.ToShortTimeString() + "-" + endTime.Value.ToShortTimeString() + " , please change appointment time" });
                    }
                }
            
            }

            if ((bool)requireConfirm)  //---> path for decision 1
            {
                warningmessage.Add("The case in between " + startTime.Value.ToShortTimeString() + " - " + endTime.Value.ToShortTimeString() + " is already equal or more than the maximum case, this might lead to work overload");
                TempData["warning"] = warningmessage;
                return Json(new
                {
                    Result = "Confirm"
                    /*  Result = "Your select time is" + startTime + " to " + endTime +
                      "But between \n" + String.Join("\n", warningmessage) + "\n is already busy. Do you still want to create the appointment? "*/
                });
            }
            else
            {
                return Json(new { Result = "Success" });
            }

        }

          public JsonResult GetWarningMessage()
            {
                if (TempData["warning"] != null)
                {
                    var ls = (List<string>)TempData["warning"];
                var result = string.Join(",", ls.ToArray());
                //     string[][] newKeys = ls.Select(x => new string[] { x }).ToArray();

                string json = JsonConvert.SerializeObject(result);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                return Json("NoWarning");
            }

        [HttpPost]
        public ActionResult CreateApp(int memberID, int? petID, int serviceID, string detail, string suggestion, DateTime date, DateTime startTime, DateTime endTime)
        {
            if (serviceID == 4 && TempData["warning"].Equals(true)) {
                return Json(new { Result = "fail" });
            }
            if (startTime == null || endTime == null)
            {
                return Json(new { Result = "Fail, start time and end time is require" });
            }
            if (petID == 0)
            {
                return Json(new { Result = "Fail, pet is require" });
            }

            var start = new DateTime(date.Year, date.Month, date.Day,
                                      startTime.Hour, startTime.Minute, 0, startTime.Kind);
                var end = new DateTime(date.Year, date.Month, date.Day,
                                        endTime.Hour, endTime.Minute, 0, endTime.Kind);

                Appointment appointment = new Appointment();
                appointment.memberId = memberID;
                appointment.petId = Int32.Parse(petID.ToString());
                appointment.serviceId = serviceID;
                appointment.detail = detail;
                appointment.suggestion = suggestion;
                appointment.startTime = start;
                appointment.endTime = end;
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



            //  List<TimeBlock> CheckTime = TimeBlockService.GetAll().ToList();
            DateTime checkStart, checkEnd;
        //    var warningmessage = new List<String>();
            foreach (var hour in hours)
            {
                //---------------> Find timeblock object to add to AppointmentTimeBlock and add case number to timeblock
                checkStart = new DateTime(date.Year, date.Month, date.Day,
                                    hour.Hour, hour.Minute, 0, startTime.Kind);
                checkEnd = checkStart.AddHours(0.5);
                TimeSlot checkTimeBlock = _TimeSlotRepo.GetByTime(checkStart, checkEnd);


                //Create AppointmentTimeblock
                AppointmentTimeSlot appTime = new AppointmentTimeSlot();
                appTime.timeId = checkTimeBlock.id;
                appTime.appointmentID = appointment.id;
                _AppTimeRepo.Add(appTime);
                //--->Update timeblock object
                checkTimeBlock.numberofCase += 1;
                _TimeSlotRepo.Update(checkTimeBlock);

                // If case is not surgery
                if (service.description != "Surgery")
                {
                    // Still not fix the number of case that can take at a time
                    if (checkTimeBlock.numberofCase >= clinic.maximumCase)
                    {
                        checkTimeBlock.status = "Busy";
                        _TimeSlotRepo.Update(checkTimeBlock);
                    }
                }
                else
                {
                    checkTimeBlock.status = "Full";
                    _TimeSlotRepo.Update(checkTimeBlock);

                }
            }

        }



        [HttpPost]
        public ActionResult Edit(int appid, string detail, string suggestion)
        {
            Appointment appointment = _AppRepo.GetById(appid);
            appointment.suggestion = suggestion;
            appointment.detail = detail;
            AppointmentService.Update(appointment);
            return Json(new { Result = "Success" });
        }

        [HttpPost]
        public ActionResult Delete(int appid)
        {
            // Clinic object
            Clinic clinic = _VCRepo.Get();


            List<AppointmentTimeSlot> appTimeblock = _AppTimeRepo.GetByAppointmentID(appid).ToList();
            if (appTimeblock != null && appTimeblock.Count > 0)
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
            Appointment app = _AppRepo.GetById(appid);
            _AppRepo.Delete(app);
            return Json(new { Result = "Success" });
        }
    }
}