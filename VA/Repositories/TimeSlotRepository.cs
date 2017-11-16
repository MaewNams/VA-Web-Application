using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(TimeSlot model)
        {
            _db.TimeSlot.Add(model);
            _db.SaveChanges();
        }


      /*  public IEnumerable<TimeSlot> GetAllFromToday(DateTime now)
        {
            IEnumerable<TimeSlot> timeList = _db.TimeSlot.Where(m => m.startTime.Date >= now.Date);
            return timeList;
        }*/

        public TimeSlot GetByDate(int day, int month, int year)
        {
            return _db.TimeSlot.FirstOrDefault(m => m.startTime.Day == day && m.startTime.Month == month && m.startTime.Year == year);
        }

        public TimeSlot GetByID(int id)
        {
            return _db.TimeSlot.FirstOrDefault(m => m.id == id);
        }

        public TimeSlot GetByTime(DateTime startTime, DateTime endTime)
        {
            return _db.TimeSlot.FirstOrDefault(m => m.startTime == startTime && m.endTime == endTime);
        }


        public IEnumerable<TimeSlot> GetListByDate(int day, int month, int year)
        {
            IEnumerable<TimeSlot> timeList = _db.TimeSlot.Where(m => m.startTime.Day == day && m.startTime.Month == month && m.startTime.Year == year);
            return timeList;
        }

     /*   public IEnumerable<TimeSlot> GetListByMonthAndYear(int month, int year)
        {
            IEnumerable<TimeSlot> timeList = _db.TimeSlot.Where(m => m.startTime.Month == month && m.startTime.Year == year);
            return timeList;
        }*/

        public void Update(TimeSlot model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}