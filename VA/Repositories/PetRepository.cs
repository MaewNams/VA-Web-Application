using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
namespace VA.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(Pet model)
        {
            _db.Pet.Add(model);
            _db.SaveChanges();
        }

        public void Delete(Pet model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public Pet GetById(int id)
        {
            return _db.Pet.FirstOrDefault(m => m.id == id);
        }
        public Pet GetByMemberIDAndNameAndSpecie(int memberId,string name,int specie)
        {
            return _db.Pet.FirstOrDefault(m => m.memberId == memberId && m.name == name && m.typeId == specie);
        }
        public IEnumerable<Pet> GetByMemberID(int memberId)
        {
            IEnumerable<Pet> petList = _db.Pet.Where(m => m.memberId == memberId).OrderBy(m => m.name);
            return petList;
        }

        public IEnumerable<Pet> GetAll()
        {
            IEnumerable<Pet> petList = _db.Pet.ToList();
            return petList;
        }

        public void Update(Pet model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}