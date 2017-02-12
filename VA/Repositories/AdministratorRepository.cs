using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
using System.Data.Entity;


namespace VA.Repositories
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly VAContext _db = new VAContext();
        public Administrator GetByUsernamrAndPassword(string username, string password)
        {

            return _db.Administrator.FirstOrDefault(s => s.username == username && s.password == password);
        }
    }
}