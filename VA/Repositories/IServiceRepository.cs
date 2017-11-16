using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;


namespace VA.Repositories
{
    public interface IServiceRepository
    {
       // void Add(VAService model);
    //    void Update(VAService model);
       // void Delete(VAService model);
        VAService GetById(int id);
        IEnumerable<VAService> GetAll();
    }
}
