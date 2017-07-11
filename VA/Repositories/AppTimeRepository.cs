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
            throw new NotImplementedException();
        }

        public IEnumerable<AppointmentTimeBlock> GetAll()
        {
            IEnumerable<AppointmentTimeBlock> appTimeList = _db.AppointmentTimeBlock;
            return appTimeList;
        }

        public void Update(AppointmentTimeBlock model)
        {
            throw new NotImplementedException();
        }
    }
}