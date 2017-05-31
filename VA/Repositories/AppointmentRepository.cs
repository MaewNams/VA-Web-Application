﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly VAContext _db = new VAContext();
        public void Add(Appointment model)
        {
            _db.Appointment.Add(model);
            _db.SaveChanges();
        }

        public void Delete(Appointment model)
        {
            _db.Entry(model).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public IEnumerable<Appointment> GetAll()
        {
            IEnumerable<Appointment> appList = _db.Appointment;
            return appList;
        }

        public IEnumerable<Appointment> GetByDayAndMonthAndYear(int day, int month, int year, string status)
        {
            IEnumerable<Appointment> appList = _db.Appointment.Where(s => s.date.Day ==day && s.date.Month == month && s.date.Year == year && s.status == status).OrderBy(s => s.date);
            return appList;
        }

        public Appointment GetById(int id)
        {
            return _db.Appointment.FirstOrDefault(m => m.id == id);
        }
        public IEnumerable<Appointment> GetByMemberId(int id, int month, int year)
        {
            IEnumerable<Appointment> appList = _db.Appointment.Where(s => s.memberId == id && s.date.Month == month && s.date.Year == year).OrderBy(s => s.date);
            return appList;
        }


        public IEnumerable<Appointment> GetByMemberIdAndStatus(int id, int month, int year, string status)
        {
            IEnumerable<Appointment> appList = _db.Appointment.Where(s => s.memberId == id && s.date.Month == month && s.date.Year == year && s.status == status).OrderBy(s=>s.date);
            return appList;
        }

        public IEnumerable<Appointment> GetByMonthAndYear(int month, int year, string status)
        {
            IEnumerable<Appointment> appList = _db.Appointment.Where(s => s.date.Month == month && s.date.Year == year && s.status == status).OrderBy(s => s.date);
            return appList;
        }

        public IEnumerable<Appointment> GetByPetID(int petID)
        {
            IEnumerable<Appointment> appList = _db.Appointment.Where(a => a.petId == petID);
            return appList;
        }

        public void Update(Appointment model)
        {
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}