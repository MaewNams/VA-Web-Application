using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.Repositories
{
    public class AppTimeRepository : IAppTimeRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(AppointmentTimeSlot model)
        {
            _db.AppointmentTimeSlot.Add(model);
            _db.SaveChanges();
        }
        public void Delete(AppointmentTimeSlot model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

       public IEnumerable<AppointmentTimeSlot> GetAll()
        {
            IEnumerable<AppointmentTimeSlot> appTimeList = _db.AppointmentTimeSlot;
            return appTimeList;
        }

        public IEnumerable<AppointmentTimeSlot> GetByAppointmentID(int id)
        {
            IEnumerable<AppointmentTimeSlot> appList = _db.AppointmentTimeSlot.Where(a => a.appointmentID == id);
            return appList;
        }


        public void Update(AppointmentTimeSlot model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}