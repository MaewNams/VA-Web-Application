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
        Member GetByPhoneNumber(string phoneNumber);
        Member GetByCodeID(string codeId);
        Member GetByID(int Id);
        IEnumerable<Member> GetAll();
        IEnumerable<Member> QueryByName(string name);
        IEnumerable<Member> QueryByAddress(string address);

    }
}
