using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    interface IAppointmentRepository
    {
        void Add(Appointment model);
        void Update(Appointment model);
        void Delete(Appointment model);
        Appointment GetById(int id);
        IEnumerable<Appointment> GetByMemberId(int id, int month, int year, string status);
        IEnumerable<Appointment> GetByMonthAndYear(int month, int year, string status);
        IEnumerable<Appointment> GetByDayAndMonthAndYear(int day,int month, int year, string status);
    }
}
