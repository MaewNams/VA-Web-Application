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
            _db.Member.Remove(model);
            _db.SaveChanges();
        }


        public Member GetByID(int Id)
        {
            return _db.Member.FirstOrDefault(m => m.id == Id);
        }

        public IEnumerable<Member> GetByName(string name)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.name.Contains(name)).OrderBy(s =>s.name);
            return memberList;
        }

        public IEnumerable<Member> GetByAddress(string address)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.address.Contains(address)).OrderBy(s => s.address);
            return memberList;
        }

        public IEnumerable<Member> GetByPhoneNumber(string phoneNumber)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.phonenumber.Contains(phoneNumber)).OrderBy(s => s.phonenumber);
            return memberList;
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

        public Member GetByEmailAndPassword(string email, string password)
        {
            return _db.Member.FirstOrDefault(m => m.email == email && m.password == password);
        }

        public Member GetByNameAndSurname( string name, string surname)
        {
            return _db.Member.FirstOrDefault(m => m.name == name && m.surname == surname);
        }

        public IEnumerable<Member> GetByEmail(string email)
        {
            IEnumerable<Member> memberList = _db.Member.Where(s => s.email.Contains(email)).OrderBy(s => s.email).ToList();
            return memberList;
        }

        public Member GetByExactlyEmail(string email)
        {
            Member member = _db.Member.FirstOrDefault(s => s.email == email);
            return member;
        }

    }
}