using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public interface IPetRepository 
    {
        void Add(Pet model);
        void Update(Pet model);
        void Delete(Pet model);
        Pet GetById(int id);
        Pet GetByType(int typeId);
        Pet GetByMemberIDAndNameAndSpecie(int memberId, string name, int specieId);
        Pet GetLast();
        IEnumerable<Pet> GetByMemberID(int memberId);
        //IEnumerable<Pet> GetAll();
    }
}
