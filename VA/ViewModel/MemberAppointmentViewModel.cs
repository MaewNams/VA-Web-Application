using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.ViewModel
{
    public class MemberAppointmentViewModel
    {
        public DateTime date { get; set; }
        public Member member { get; set; }
        public IEnumerable<Appointment> waitAppointments { get; set; }
        public IEnumerable<Appointment> comAppointments { get; set; }
    }
}