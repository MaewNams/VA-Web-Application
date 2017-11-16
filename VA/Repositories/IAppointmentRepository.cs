using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public interface IAppointmentRepository
    {
        void Add(Appointment model);
        void Update(Appointment model);
        void Delete(Appointment model);
        Appointment GetById(int id);
        IEnumerable<Appointment> GetByPetID(int petID);
        IEnumerable<Appointment> GetByMemberId(int id);
        IEnumerable<Appointment> GetByMemberIdWithMothAndYearAndStatus(int id, int month, int year, string status);
        IEnumerable<Appointment> GetByMonthAndYearAndStatus(int month, int year, string status);
        IEnumerable<Appointment> GetByDayAndMonthAndYearAndStatus(int day,int month, int year, string status);

        //Use in mobile application
    //    IEnumerable<Appointment> GetByMemberIdAndDayAndMonthAndYearNoStatus(int id, int month, int year, int day);
        IEnumerable<Appointment> GetByMemberIdAndMonthAndYearNoStatus(int id, int month, int year);
    }
}
