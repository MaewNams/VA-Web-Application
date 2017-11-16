using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class SpecieRepository : ISpecieRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(Specie model)
        {
            _db.Specie.Add(model);
            _db.SaveChanges();
        }

        public void Delete(Specie model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public IEnumerable<Specie> GetAll()
        {
            IEnumerable<Specie> SpecieList = _db.Specie.OrderBy(p => p.id);
            return SpecieList;
        }

        public Specie GetById(int id)
        {
            return _db.Specie.FirstOrDefault(m => m.id == id);
        }

        public Specie GetByName(string name)
        {
            return _db.Specie.FirstOrDefault(m => m.name == name);
        }


        public void Update(Specie model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}