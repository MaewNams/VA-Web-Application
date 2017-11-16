using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class VAServiceRepository : IServiceRepository
    {
        private readonly VAContext _db = new VAContext();

     /*   public void Add(VAService model)
        {
            _db.Service.Add(model);
            _db.SaveChanges();
        }

        public void Delete(VAService model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }
        */
        public IEnumerable<VAService> GetAll()
        {
            IEnumerable<VAService> serviceList = _db.Service.ToList();
            return serviceList;
        }

        public VAService GetById(int id)
        {
            return _db.Service.FirstOrDefault(m => m.id == id);
        }

       /* public void Update(VAService model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }*/
    }
}