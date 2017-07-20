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
        public void Add(AppointmentTimeBlock model)
        {
            _db.AppointmentTimeBlock.Add(model);
            _db.SaveChanges();
        }
        public void Delete(AppointmentTimeBlock model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public IEnumerable<AppointmentTimeBlock> GetAll()
        {
            IEnumerable<AppointmentTimeBlock> appTimeList = _db.AppointmentTimeBlock;
            return appTimeList;
        }

        public IEnumerable<AppointmentTimeBlock> GetByAppointmentID(int id)
        {
            IEnumerable<AppointmentTimeBlock> appList = _db.AppointmentTimeBlock.Where(a => a.appointmentID == id);
            return appList;
        }

        public AppointmentTimeBlock GetLast()
        {
            return _db.AppointmentTimeBlock.OrderByDescending(m => m.id).FirstOrDefault();
        }

        public void Update(AppointmentTimeBlock model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}