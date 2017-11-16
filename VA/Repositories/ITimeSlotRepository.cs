using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;


namespace VA.Repositories
{
    public interface ITimeSlotRepository
    {
        void Add(TimeSlot model);
        void Update(TimeSlot model);
        TimeSlot GetByID(int id);
        TimeSlot GetByDate(int day, int month, int year);
        TimeSlot GetByTime(DateTime startTime, DateTime endTime);
        IEnumerable<TimeSlot> GetListByDate(int day, int month, int year);
      //  IEnumerable<TimeSlot> GetListByMonthAndYear(int month, int year);
    }
}
