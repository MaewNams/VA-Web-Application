using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public interface IMemberRepository
    {
        void Add(Member model);
        void Update(Member model);
        void Delete(Member model);
        Member GetByID(int Id);
        Member GetByIDAndEmail(int id,string email);
        Member GetByEmailAndPassword(string email, string password);
        Member GetByNameAndSurname(string name, string surname);
        IEnumerable<Member> GetByEmail(string email);
        IEnumerable<Member> GetByPhoneNumber(string phoneNumber);
        IEnumerable<Member> GetByName(string name);
        IEnumerable<Member> GetByAddress(string address);
        IEnumerable<Member> GetAll();

    }
}
