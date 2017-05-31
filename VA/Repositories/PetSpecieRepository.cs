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
            throw new NotImplementedException();
        }

        public void Delete(PetType model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PetType> GetAll()
        {
            IEnumerable<PetType> petTypeList = _db.PetType;
            return petTypeList;
        }

        public PetType GetById(int id)
        {
            return _db.PetType.FirstOrDefault(m => m.id == id);
        }

        public void Update(PetType model)
        {
            throw new NotImplementedException();
        }
    }
}