using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class VCRepository : IVCRepository
    {
        private readonly VAContext _db = new VAContext();

        public void Add(Clinic model)
        {
            _db.Clinic.Add(model);
            _db.SaveChanges();
        }
        public void Update(Clinic model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public Clinic Get()
        {
            Clinic clinic = _db.Clinic.First();
            return clinic;
        }
    }
}