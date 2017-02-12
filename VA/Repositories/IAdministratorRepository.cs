using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.DAL;
using VA.Models;

namespace VA.Repositories
{
    interface IAdministratorRepository
    {
        Administrator GetByUsernamrAndPassword(string username, string password);
    }
}
