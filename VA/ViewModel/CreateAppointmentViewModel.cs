using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.DAL;
using VA.Models;
using VA.Repositories;

namespace VA.ViewModel
{
    public class CreateAppointmentViewModel
    {
        public DateTime date { get; set; }
        public Member member { get; set; }
        public IEnumerable<TimeBlock> timeblocks { get; set; }
        public IEnumerable<Pet> pets { get; set; }
        public IEnumerable<VAService> services { get; set; }
    }
}