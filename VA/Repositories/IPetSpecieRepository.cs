using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;
namespace VA.Repositories
{
    interface IPetSpecieRepository
    {
        void Add(PetType model);
        void Update(PetType model);
        void Delete(PetType model);
        PetType GetById(int id);
        IEnumerable<PetType> GetAll();
    }
}
