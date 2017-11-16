using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.Service
{
    public class CalendarSchedule
    {
       // private static VAContext _db = new VAContext();
        private static ITimeSlotRepository _TimeSlotRepo;


        public CalendarSchedule(ITimeSlotRepository timeSlotRepository)
        {
            _TimeSlotRepo = timeSlotRepository;
        }

        public static void Start()
        {

            DateTime current = DateTime.Now;
            int day = DateTime.Now.Day;
          //  DateTime year;

            TimeSpan start = new TimeSpan(09, 30, 0);
            TimeSpan end = new TimeSpan(21, 30, 0);


            // Check if the system already have  today time table  -- if not, create this month + five month = 6 month//
            TimeSlot checkTodayExits = _TimeSlotRepo.GetByDate(current.Day, current.Month, current.Year);

            //To day is not exits --> Create for six month
            if (checkTodayExits == null) {
                int totalmonth = current.Month + 5;
                //Loop for this month + next 5 month
                for (int i = current.Month; i <= totalmonth; i++)
                {
                    //Loop for day in month
                    for (int j = 1; j <= DateTime.DaysInMonth(current.Year, i); j++)
                    {
                        //Start from today
                        DateTime dt = new DateTime(current.Year, current.Month, j);
                        TimeSlot checkExits = _TimeSlotRepo.GetByDate(dt.Day, dt.Month, dt.Year);

                        //Loop for hour in day
                        if (checkExits == null)
                        {

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
                                Console.WriteLine("From " + timeblock.startTime + " To " + timeblock.endTime);
                            }
                        }
                    }
                }
            }

                //If current date is 1 we will create the 5 month from now timetable
                if (day == 1)
            {
                DateTime NewMonth = DateTime.Now.AddMonths(5);
                for (int i = 1; i <= DateTime.DaysInMonth(NewMonth.Year, NewMonth.Month); i++)
                {
                    DateTime dt = new DateTime(NewMonth.Year, NewMonth.Month, i);
                    TimeSlot checkExits = _TimeSlotRepo.GetByDate(dt.Day, dt.Month, dt.Year);

                    if (checkExits == null)
                    {

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
                    }

                }

            }
            ///
        }

    }
}