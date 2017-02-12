using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(Member model)
        {
            _db.Member.Add(model);
            _db.SaveChanges();
        }

        public void Delete(Member model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public Member GetByCodeID(string codeId)
        {
            return _db.Member.FirstOrDefault(m => m.codeId == codeId);
        }

        public Member GetByID(int Id)
        {
            return _db.Member.FirstOrDefault(m => m.id == Id);
        }

        public IEnumerable<Member> QueryByName(string name)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.name.Contains(name));
            return memberList;
        }

        public IEnumerable<Member> QueryByAddress(string address)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.address.Contains(address));
            return memberList;
        }

        public Member GetByPhoneNumber(string phoneNumber)
        {
            return _db.Member.FirstOrDefault(m => m.phonenumber == phoneNumber);
        }

        public void Update(Member model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public IEnumerable<Member> GetAll()
        {
            IEnumerable<Member> memberList = _db.Member.ToList();
            return memberList;
        }

        /*     IEnumerable<Appointment> XXXX(int month, int year)
             {
                 IEnumerable<Appointment> nameList = _db.Appointment.Where(s=>s.date.Month == month && s.date.Year == year);
                 return nameList;
             }
             */
    }
}