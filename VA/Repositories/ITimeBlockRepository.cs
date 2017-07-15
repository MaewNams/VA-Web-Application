using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;


namespace VA.Repositories
{
    interface ITimeBlockRepository
    {
        void Add(TimeBlock model);
        void Update(TimeBlock model);
        void Delete(TimeBlock model);
        TimeBlock GetLast();
        TimeBlock GetByDate(int day, int month, int year);
        TimeBlock GetByTime(DateTime startTime, DateTime endTime);
        IEnumerable<TimeBlock> GetAll();
        IEnumerable<TimeBlock> GetAllNotFull();
        IEnumerable<TimeBlock> GetListByDate(int day, int month, int year);
        IEnumerable<TimeBlock> GetListByMonthAndYear(int month, int year);
    }
}
