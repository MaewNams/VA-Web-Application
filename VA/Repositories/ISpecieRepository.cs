using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;
namespace VA.Repositories
{
    public interface ISpecieRepository
    {
        void Add(Specie model);
        void Update(Specie model);
        void Delete(Specie model);
        Specie GetById(int id);
        Specie GetByName(string name);
        IEnumerable<Specie> GetAll();
    }
}
