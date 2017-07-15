using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class TimeBlockRepository : ITimeBlockRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(TimeBlock model)
        {
            _db.TimeBlock.Add(model);
            _db.SaveChanges();
        }

        public void Delete(TimeBlock model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimeBlock> GetAll()
        {
            IEnumerable<TimeBlock> timeList = _db.TimeBlock;
            return timeList;
        }

        public IEnumerable<TimeBlock> GetAllNotFull()
        {
            IEnumerable<TimeBlock> timeList = _db.TimeBlock.Where(m => m.status != "Full");
            return timeList;
        }

        public TimeBlock GetByDate(int day, int month, int year)
        {
            return _db.TimeBlock.FirstOrDefault(m => m.startTime.Day == day && m.startTime.Month == month && m.startTime.Year == year);
        }

        public TimeBlock GetByTime(DateTime startTime, DateTime endTime)
        {
            return _db.TimeBlock.FirstOrDefault(m => m.startTime == startTime && m.endTime == endTime);
        }

        public TimeBlock GetLast()
        {
            return _db.TimeBlock.OrderByDescending(m => m.id).FirstOrDefault();
        }

        public IEnumerable<TimeBlock> GetListByDate(int day, int month, int year)
        {
            IEnumerable<TimeBlock> timeList = _db.TimeBlock.Where(m => m.startTime.Day == day && m.startTime.Month == month && m.startTime.Year == year);
            return timeList;
        }

        public IEnumerable<TimeBlock> GetListByMonthAndYear(int month, int year)
        {
            IEnumerable<TimeBlock> timeList = _db.TimeBlock.Where(m => m.startTime.Month == month && m.startTime.Year == year);
            return timeList;
        }

        public void Update(TimeBlock model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}