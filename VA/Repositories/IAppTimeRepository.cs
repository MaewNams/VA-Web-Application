using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    interface IAppTimeRepository
    {
        void Add(AppointmentTimeBlock model);
        void Update(AppointmentTimeBlock model);
        void Delete(AppointmentTimeBlock model);
        AppointmentTimeBlock GetLast();
        IEnumerable<AppointmentTimeBlock> GetAll();
        IEnumerable<AppointmentTimeBlock> GetByAppointmentID(int id);
    }
}
