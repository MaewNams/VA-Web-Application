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
        Member GetByCodeIDAndPassword(string codeId, string password);
        Member GetByNameAndSurname(string name, string surname);
        IEnumerable<Member> GetByPhoneNumber(string phoneNumber);
        IEnumerable<Member> GetByCodeID(string codeId);
        IEnumerable<Member> GetAll();
        IEnumerable<Member> QueryByName(string name);
        IEnumerable<Member> QueryByAddress(string address);

    }
}
