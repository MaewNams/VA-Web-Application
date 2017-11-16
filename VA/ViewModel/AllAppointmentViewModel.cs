using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
using VA.Repositories;
namespace VA.ViewModel
{
    public class AllAppointmentViewModel
    {   /// View all appointment in main page
        public DateTime date { get; set; }
        public IEnumerable<Appointment> AppWait { get; set; }
        public IEnumerable<Appointment> AppCom { get; set; }
    }
}