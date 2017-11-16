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
<<<<<<< HEAD
        Pet GetById(int id);
        Pet GetBySpecie(int specieId);
=======
       Pet GetById(int id);
        Pet GetByType(int typeId);
>>>>>>> 77357a911c4c8b10fde568890d9b5e97c99aa41a
        Pet GetByMemberIDAndNameAndSpecie(int memberId, string name, int specieId);
        IEnumerable<Pet> GetByMemberID(int memberId);
    }
}
