using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class PetSpecieRepository : IPetSpecieRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(PetType model)
        {
            _db.PetType.Add(model);
            _db.SaveChanges();
        }

        public void Delete(PetType model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public IEnumerable<PetType> GetAll()
        {
            IEnumerable<PetType> petTypeList = _db.PetType.OrderBy(p => p.name);
            return petTypeList;
        }

        public PetType GetById(int id)
        {
            return _db.PetType.FirstOrDefault(m => m.id == id);
        }

        public PetType GetByName(string name)
        {
            return _db.PetType.FirstOrDefault(m => m.name == name);
        }

        public PetType GetLast()
        {
            return _db.PetType.OrderByDescending(m => m.id).FirstOrDefault();
        }

        public void Update(PetType model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}