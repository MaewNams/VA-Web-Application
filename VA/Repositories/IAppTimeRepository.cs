using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public interface IAppTimeRepository
    {
        void Add(AppointmentTimeSlot model);
        void Update(AppointmentTimeSlot model);
        void Delete(AppointmentTimeSlot model);
        IEnumerable<AppointmentTimeSlot> GetAll();
        IEnumerable<AppointmentTimeSlot> GetByAppointmentID(int id);
    }
}
